using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.MatchUseCase
{
    public interface IMatchUseCase
    {

        Task<IEnumerable<Match>> GetAllMatches(int? roundId);
        Task<IEnumerable<Match>> GetOpenMatches(int? roundId);
        Task<IEnumerable<Match>> GetPlayedMatches(int? roundId);
        Task<Player?> GetWinnerOfMatch(int matchId);
        Task<IEnumerable<Player?>?> GetAllWinnersOfRound(int roundId);
        Task<Match> RescheduleAMatch(int matchId, DateOnly rescheduleDate);
        
    }
}