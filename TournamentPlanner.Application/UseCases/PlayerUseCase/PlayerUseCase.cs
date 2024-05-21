using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.PlayerUseCase
{
    public class PlayerUseCase : IPlayerUseCase
    {
        public IRepository<Player, Player> _playerRepository { get; }

        public PlayerUseCase(IRepository<Player, Player> playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<Player> AddPlayerAsync(PlayerDto playerDto)
        {
            var player = FromDto(playerDto);
            await _playerRepository.AddAsync(player);
            await _playerRepository.SaveAsync();
            return player;
        }

        public Task<IEnumerable<Player>> GetAllPlayerWhoseMatchNotStillPlayedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Player>?> GetPlayersAsync(string? playerName)
        {
            //var player = await _playerRepository.GetAllAsync(["Tournament"]);
            if (playerName is null) return await _playerRepository.GetAllAsync();

            return await _playerRepository.GetByNameAsync(playerName);
        }

        public PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email,
                TournamentId = player.TournamentId,
            };
        }

        public Player FromDto(PlayerDto dto)
        {
            return new Player
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                TournamentId = dto.TournamentId,
            };
        }

        public async Task<Player> GetPlayerById(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            if(player == null)
            {
                throw new Exception("Player with the speficied Id not found!");
            }
            return player;
        }
    }
}
