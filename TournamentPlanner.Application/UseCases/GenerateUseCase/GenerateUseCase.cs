using System;
using System.Collections.Generic;
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
        private readonly IRepository<PlayerDto, Player> _playerRepository;
        private readonly IRepository<Match, Match> _matchRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Tournament, Tournament> _tournamentRepository;
        private readonly IRepository<Round, Round> _roundRepository;

        public GenerateUseCase(IRepository<PlayerDto, Player> playerRepository, IRepository<Match, Match> matchRepository, IRepository<Tournament, Tournament> tournamentRepository, IRepository<Round, Round> roundRepository, IConfiguration configuration)
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
                var playerDto = ToDto(player);
                playerDto.Tournament = tournament;
                await _playerRepository.AddAsync(playerDto);
            }

            await _playerRepository.SaveAsync();

            return await _playerRepository.GetAllAsync();
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
            // var rounds = await _roundRepository.GetAllAsync(new string[] {""})
            // damn it, cant do this here, need to declare it in interface

            //check if we have 32 palyers in this tournament
            if( typeof(T) == typeof(string)){
                var rounds = await _roundRepository.GetByNameAsync(TournamentIdentifier.ToString());
            }

            if( typeof(T) == typeof(int)){
                throw new NotImplementedException();
            }

            //for this we need round repository, lets go make it

            //figure out the round
            // if no match played, next round is 1st round
            // if 16 match played, next round is 2nd round
            // if 24 match played, next round is 3rd round
            // if 28 match played, next round is 4th round
            // if 30 match played, next round is 5th and final round


            //after figuring out the round, take all the winner from the previous round
            // take all players if round 1

            // take two random player and make a match between them


            //can be done later,, schedule 8match at max each day.
            //Assign schedule based on tournament start day
            throw new NotImplementedException();
        }

        public Task<List<Match>> SimulateMatches<T>(T TournamentIdentifier)
        {
            //figure out which round, is going on
            //take all the matches and assign a random winner
            throw new NotImplementedException();
        }
    }
}