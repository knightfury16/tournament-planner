using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<Player> AddPlayerAsync(PlayerDto player)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Player>> GetAllPlayerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Player>> GetAllPlayerWhoseMatchNotStillPlayedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Player>> GetPlayersAsync(string? playerName)
        {
            throw new NotImplementedException();
        }
    }
}
