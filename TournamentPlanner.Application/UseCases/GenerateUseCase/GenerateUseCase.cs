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
        public async Task<IEnumerable<Player>> AddPlayerAutoToTournament(string TournamentName)
        {
            var tournament = await AddNewTournament(TournamentName);

            List<Player> playerList = await GetAllPlayers();

            //Repository limitation, no method to add in bulk
            foreach (Player player in playerList)
            {
                player.Tournament = tournament;
                var addedPlayer = await _playerRepository.AddAsync(player);
            }

            await _playerRepository.SaveAsync();

            return await _playerRepository.GetAllAsync(["Tournament"]);
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

        public async Task<List<Match>> MakeRoaster<T>(T TournamentIdentifier)
        {
            IEnumerable<Round> rounds;

            if (typeof(T) == typeof(string))
            {
                var tournamentName = TournamentIdentifier as string;

                if (tournamentName is null)
                {
                    throw new Exception("Can not convert Tournament Identifier to string");
                }

                //figure out the round
                //Get rounds, if we have any, game has been played before
                rounds = await _roundRepository.GetAllAsync(r => r.Tournament?.Name == tournamentName, ["Tournament", "Matches"]);

                //else its first round, check for player constrain
                if (rounds.Count() == 0)
                {
                    //check if we have 32 palyers in this tournament
                    var players = await CheckAndGetPlayerCountInTournament(tournamentName);
                    if (players == null)
                    {
                        throw new Exception("String Tournament Identifier: Insufficiant player to make schedule");
                    }

                    //we have 32 player and its first round, make a roaster
                    IEnumerable<Match> matches = await MakeFirstMatchRoaster(players, tournamentName);

                    return (List<Match>)await _matchRepository.GetAllAsync();
                }
                else
                {
                    var maxRound = rounds.MaxBy(r => r.RoundNumber);
                    if (maxRound != null)
                    {
                        var matchesPlayed = GetTheMatchesPlayed(maxRound.Matches);
                        IEnumerable<Match> nextRoundMatches;
                        // if 16 match played, next round is 2nd round
                        if (matchesPlayed.Count() == 16)
                        {
                            nextRoundMatches = await MakeMatchRoaster(matchesPlayed, tournamentName, 2);
                        }
                        // if 24 match played, next round is 3rd round
                        else if (matchesPlayed.Count() == 24)
                        {
                            nextRoundMatches = await MakeMatchRoaster(matchesPlayed, tournamentName, 3);
                        }
                        // if 28 match played, next round is 4th round
                        else if (matchesPlayed.Count() == 28)
                        {
                            nextRoundMatches = await MakeMatchRoaster(matchesPlayed, tournamentName, 4);
                        }
                        else
                        {
                            // if 30 match played, next round is 5th and final round
                            System.Console.WriteLine("TODO");
                        }
                    }


                }

            }

            if (typeof(T) == typeof(int))
            {
                if (TournamentIdentifier is int)
                {
                    var tournamentId = Convert.ToInt32(TournamentIdentifier);
                    var TRounds = await _roundRepository.GetAllAsync(r => r.TournamentId == tournamentId);
                }
            }



            // List<Round> rounds = new();

            // var round = rounds.MaxBy(r => r.RoundNumber);

            // if (round != null)
            // {
            //     var matchPlayed = round.Matches.Count();
            // }



            //after figuring out the round, take all the winner from the previous round
            // take all players if round 1

            // take two random player and make a match between them


            //can be done later,, schedule 8match at max each day.
            //Assign schedule based on tournament start day
            throw new NotImplementedException();
        }

        private List<Match> GetTheMatchesPlayed(List<Match> matches)
        {
            return matches.Where(m => m.IsComplete == false).ToList();
        }

        private async Task<IEnumerable<Match>> MakeMatchRoaster(List<Match> completedMatches, string tournamentName, int roundNumber)
        {

            List<Match> matches = new List<Match>();
            var tournament = await _tournamentRepository.GetByNameAsync(tournamentName);

            var nextRound = new Round
            {
                RoundNumber = roundNumber,
                Tournament = tournament?.First(),
            };

            await _roundRepository.AddAsync(nextRound);

            var winnerPlayers = completedMatches.Select(m => m.Winner).ToList();

            //Shuffle the player for randomness
            var random = new Random();
            winnerPlayers = winnerPlayers.OrderBy(p => random.Next()).ToList();


            if (winnerPlayers != null && winnerPlayers.Count > 0)
            {

                //create 16 pairs of players
                for (int i = 0; i < winnerPlayers.Count / 2; i++)
                {
                    Player player1 = winnerPlayers[i * 2];
                    Player player2 = winnerPlayers[(i * 2) + 1];

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
                await _matchRepository.SaveAsync();
            }

            return matches;
        }

        private async Task<IEnumerable<Match>> MakeFirstMatchRoaster(List<Player> players, string tournamentName)
        {
            var tournament = await _tournamentRepository.GetByNameAsync(tournamentName);

            var round1 = new Round
            {
                RoundNumber = 1,
                Tournament = tournament?.First(),
            };

            await _roundRepository.AddAsync(round1);

            //we have 32 player
            //we need to make 16 pairs and make a match between them

            //Shuffle the player for randomness
            var random = new Random();
            players = players.OrderBy(p => random.Next()).ToList();


            //Create 16 pairs of players
            List<Match> matches = new List<Match>();
            for (int i = 0; i < players.Count / 2; i++)
            {
                Player player1 = players[i * 2];
                Player player2 = players[(i * 2) + 1];

                var match = new Match
                {
                    FirstPlayer = player1,
                    SecondPlayer = player2,
                    IsComplete = false,
                    Round = round1
                };
                matches.Add(match);
                await _matchRepository.AddAsync(match);
            }
            await _matchRepository.SaveAsync();
            return matches;
        }


        //* Tournament Identifier is string
        private async Task<List<Player>?> CheckAndGetPlayerCountInTournament(string tournamentName)
        {
            var allPlayers = await _playerRepository.GetAllAsync(p => p.Tournament?.Name == tournamentName, ["Tournament"]);
            if (allPlayers.Count() == PLAYER_COUNT)
            {
                return (List<Player>?)allPlayers;
            }
            else
            {
                return null;
            }
        }

        //* Tournament Identifier is int
        private async Task<bool> CheckAndGetPlayerCountInTournament(int tournamentId)
        {
            var allPlayers = await _playerRepository.GetAllAsync(p => p.TournamentId == tournamentId);

            return allPlayers.Count() == PLAYER_COUNT;
        }

        public Task<List<Match>> SimulateMatches<T>(T TournamentIdentifier)
        {
            //figure out which round, is going on
            //take all the matches and assign a random winner
            throw new NotImplementedException();
        }
    }
}