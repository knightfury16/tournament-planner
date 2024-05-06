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
            if (PlayerValidation(playerDto))
            {
                var player = FromDto(playerDto);
                await _playerRepository.AddAsync(player);
                await _playerRepository.SaveAsync();
                return player;
            }
            else
            {
                throw new ArgumentException("Invalid parameter");
            }
        }

        public Task<IEnumerable<Player>> GetAllPlayerWhoseMatchNotStillPlayedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(string? playerName)
        {
            var player = await _playerRepository.GetAllAsync(["Tournament"]);
            //var player = await _playerRepository.GetAllAsync();
            if (player == null)
            {
                throw new ArgumentException("No player found");
            }
            return player;
        }

        private bool PlayerValidation(PlayerDto playerDto)
        {
            //mock validation
            try
            {
                if (playerDto == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(playerDto.Name))
                {
                    return false;
                }

                var mailAddress = playerDto.Email != null ? new MailAddress(playerDto.Email) : null;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email,
                Tournament = player.Tournament,
            };
        }

        public Player FromDto(PlayerDto dto)
        {
            return new Player
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Tournament = dto.Tournament,
            };
        }
    }
}
