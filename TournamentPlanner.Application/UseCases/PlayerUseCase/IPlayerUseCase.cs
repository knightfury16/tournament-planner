using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.AddPlayer
{
    //Lets not separate the concern for now. 
    // TODO: Separate command and query
    public interface IPlayerUseCase
    {
        Task<Player> AddPlayerAsync(PlayerDto player);
        Task<IEnumerable<Player>> GetAllPlayerAsync();
        Task<IEnumerable<Player>> GetAllPlayerWhoseMatchNotStillPlayedAsync();
        Task<IEnumerable<Player>> GetPlayersAsync(string? playerName);
    }
}
