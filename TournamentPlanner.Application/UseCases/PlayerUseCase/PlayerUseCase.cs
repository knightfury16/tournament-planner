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
        public IRepository<PlayerDto, Player> _playerRepository { get; }

        public PlayerUseCase(IRepository<PlayerDto, Player> playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<Player> AddPlayerAsync(PlayerDto playerDto)
        {
            if (PlayerValidation(playerDto))
            {
                var player = await _playerRepository.AddAsync(playerDto);
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
            var allPlayer = await _playerRepository.GetAllAsync();

            if (playerName != null)
            {
                allPlayer = allPlayer.Where(p => p.Name.Contains(playerName));
            }

            return allPlayer;
        }

        private bool PlayerValidation(PlayerDto playerDto)
        {
            //mock validation
            try
            {
                if(playerDto == null)
                {
                    return false;
                }
                if(string.IsNullOrEmpty(playerDto.Name)) {
                    return false;
                }
                var mailAddress = new MailAddress(playerDto.Email);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
