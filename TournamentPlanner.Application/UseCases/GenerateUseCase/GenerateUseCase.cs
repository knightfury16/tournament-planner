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

        public GenerateUseCase(IRepository<PlayerDto, Player> playerRepository, IRepository<Match, Match> matchRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _playerRepository = playerRepository;
            _matchRepository = matchRepository;
        }
        public async Task<List<Player>> AddPlayerAutoToTournament(string TournamentName)
        {
            List<Player> playerList = await GetAllPlayers();

            //Repository limitation, no method to add in bulk
            foreach (Player player in playerList)
            {
                var playerDto = ToDto(player);
                await _playerRepository.AddAsync(playerDto);
            }

            await _playerRepository.SaveAsync();

            return playerList;
        }

        //TODO: make a util, or add auto mapper
        private PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email
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

        public Task<List<Match>> MakeRoaster<T>(T TournamentIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<List<Match>> SimulateMatches<T>(T TournamentIdentifier)
        {
            throw new NotImplementedException();
        }
    }
}