using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.GenerateUseCase
{
    public class GenerateUseCase : IGenerate
    {
        private readonly IRepository<Player, Player> _playerRepository;
        private readonly IRepository<Match, Match> _matchRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Tournament, Tournament> _tournamentRepository;
        private readonly IRepository<Round, Round> _roundRepository;
        private int PLAYER_COUNT { get; } = 32;

        public GenerateUseCase(IRepository<Player, Player> playerRepository, IRepository<Match, Match> matchRepository, IRepository<Tournament, Tournament> tournamentRepository, IRepository<Round, Round> roundRepository, IConfiguration configuration)
        {
            _tournamentRepository = tournamentRepository;
            _roundRepository = roundRepository;
            _configuration = configuration;
            _playerRepository = playerRepository;
            _matchRepository = matchRepository;
        }
        public async Task<IEnumerable<Player>> AddTournamentAndPlayerAuto(string tournamentName)
        {
            var tournament = await AddNewTournament(tournamentName);

            List<Player> playerList = await GetAllPlayers();

            //Repository limitation, no method to add in bulk
            foreach (Player player in playerList)
            {
                player.Tournament = tournament;
                var addedPlayer = await _playerRepository.AddAsync(player);
            }

            await _playerRepository.SaveAsync();

            return await _playerRepository.GetAllAsync(p => p.TournamentId == tournament.Id ,["Tournament"]);
        }

        private async Task<Tournament> AddNewTournament(string tournamentName)
        {
            var tournament = new Tournament { Name = tournamentName };
            await _tournamentRepository.AddAsync(tournament);
            await _tournamentRepository.SaveAsync();
            return tournament;
        }

        //TODO: make a util, or add auto mapper
        private PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email,
            };
        }


        private async Task<List<Player>> GetAllPlayers()
        {
            var playerDataPath = _configuration["playerData"];

            if (playerDataPath == null)
            {
                throw new Exception("Player Data could not be found");
            }
            var playerJsonContent = await File.ReadAllTextAsync(playerDataPath);

            if (playerJsonContent == null)
            {
                throw new Exception("Player Content could not be read");
            }

            var playerList = JsonSerializer.Deserialize<List<Player>>(playerJsonContent);

            if (playerList == null)
            {
                throw new Exception("Player could not be deserialized");
            }

            return playerList;

        }

        public async Task<List<Match>?> MakeRoaster(int tournamentId)
        {
            var maxRound = await GetMaxRound(tournamentId);

            //its first round, check for player constrain
            if (maxRound is null)
            {
                //check if we have 32 palyers in this tournament
                var players = await CheckAndGetPlayerCountInTournament(tournamentId);
                if (players == null)
                {
                    throw new Exception("Insufficiant player to make schedule");
                }

                //we have 32 player and its first round, make a roaster
                IEnumerable<Match> matches = await MakeRoundRoaster(null, tournamentId, 1);

                return (List<Match>)matches;
            }
            else
            {
                if (maxRound.RoundNumber >= 5)
                {
                    throw new Exception("Tournament Already Finish.Can not make roaster");
                }

                var matchesPlayed = GetTheMatchesPlayed(maxRound.Matches);

                IEnumerable<Match>? nextRoundMatches = null;

                // if 16 match played, next round is 2nd round
                if (maxRound.Matches.Count() == 16 && maxRound.Matches.Count() == matchesPlayed.Count())
                {
                    nextRoundMatches = await MakeRoundRoaster(matchesPlayed, tournamentId, 2);
                }
                // if 24 match played, next round is 3rd round
                else if (maxRound.Matches.Count() == 8 && maxRound.Matches.Count() == matchesPlayed.Count())
                {
                    nextRoundMatches = await MakeRoundRoaster(matchesPlayed, tournamentId, 3);
                }
                // if 28 match played, next round is 4th round
                else if (maxRound.Matches.Count() == 4 && maxRound.Matches.Count() == matchesPlayed.Count())
                {
                    nextRoundMatches = await MakeRoundRoaster(matchesPlayed, tournamentId, 4);
                }
                //final round, 5th
                else if (maxRound.Matches.Count() == 2 && maxRound.Matches.Count() == matchesPlayed.Count())
                {
                    nextRoundMatches = await MakeRoundRoaster(matchesPlayed, tournamentId, 5);
                }


                if (nextRoundMatches is null)
                {
                    return null;
                }

                return (List<Match>)nextRoundMatches;

            }

            //can be done later,, schedule 8match at max each day.
            //Assign schedule based on tournament start day

        }

        private async Task<Round?> GetMaxRound(int tournamentId)
        {
            //figure out the round
            //Get rounds, if we have any, game has been played before
            var rounds = await _roundRepository.GetAllAsync(r => r.TournamentId == tournamentId, ["Matches"]);

            var maxRound = rounds.MaxBy(r => r.RoundNumber);

            if (maxRound != null)
            {
                //Calling this to populate players
                //it does not make sense here to have matches and not populate the players
                await _matchRepository.GetAllAsync(m => m.RoundId == maxRound.Id, ["FirstPlayer", "SecondPlayer"]);
            }
            //check if we populated winner by default
            //Yah they populate automatically
            return maxRound;
        }

        private List<Match> GetTheMatchesPlayed(List<Match> matches)
        {
            return matches.Where(m => m.IsComplete == true).ToList();
        }

        private async Task<IEnumerable<Match>> MakeRoundRoaster(List<Match>? completedMatches, int tournamentId, int roundNumber)
        {

            List<Match> matches = new List<Match>();
            List<Player>? eligiblePlayers = new();
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);

            var nextRound = new Round
            {
                RoundNumber = roundNumber,
                Tournament = tournament,
                StartTime = DateTime.UtcNow
            };

            await _roundRepository.AddAsync(nextRound);

            if (completedMatches == null)
            {
                eligiblePlayers = (List<Player>)await _playerRepository.GetAllAsync(p => p.TournamentId == tournamentId);
            }
            else
            {
                eligiblePlayers = completedMatches.Select(m => m.Winner).ToList();
            }


            //Shuffle the player for randomness
            var random = new Random();
            eligiblePlayers = eligiblePlayers.OrderBy(p => random.Next()).ToList();


            if (eligiblePlayers != null && eligiblePlayers.Count > 0)
            {

                //create 16 pairs of players
                for (int i = 0; i < eligiblePlayers.Count / 2; i++)
                {
                    Player player1 = eligiblePlayers[i * 2];
                    Player player2 = eligiblePlayers[(i * 2) + 1];

                    var match = new Match
                    {
                        FirstPlayer = player1,
                        SecondPlayer = player2,
                        IsComplete = false,
                        Round = nextRound
                    };
                    matches.Add(match);
                    await _matchRepository.AddAsync(match);
                }
            //Match make up complete schedule the matches now.
            // Scheduler.schedule(matches,nextRound)
                matches = Scheduler.Schedule(matches,nextRound);

                await _matchRepository.SaveAsync();
            }

            return matches;
        }

        private async Task<List<Player>?> CheckAndGetPlayerCountInTournament(int tournamentId)
        {
            var allPlayers = await _playerRepository.GetAllAsync(p => p.TournamentId == tournamentId);
            if (allPlayers.Count() == PLAYER_COUNT)
            {
                return (List<Player>?)allPlayers;
            }
            else
            {
                return null;
            }
        }


        public async Task<List<Match>?> SimulateMatches(int tournamentId, bool allMatch = false)
        {
            //figure out which round, is going on

            var maxRound = await GetMaxRound(tournamentId);

            if (maxRound == null) return null;

            var matches = maxRound.Matches;

            if (allMatch)
            {
                matches = WinnerGenerator.MakeAllMatchRadomWinner(matches);
            }
            else
            {
                matches = WinnerGenerator.MakeSomeMatchRandomWinner(matches);
            }
            await _matchRepository.SaveAsync();
            return matches;

        }
    }
}