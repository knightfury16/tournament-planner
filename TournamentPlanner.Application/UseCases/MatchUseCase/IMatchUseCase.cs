using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.MatchUseCase
{
    public interface IMatchUseCase
    {

        Task<IEnumerable<Match>> GetAllRoundMatches(int roundId);
        Task<IEnumerable<Match>> GetAllTournamentMatches(int tournamentId);
        Task<IEnumerable<Match>> GetAllMatches();
        Task<IEnumerable<Match>> GetOpenMatches(int? roundId, int? tournamentId);
        Task<IEnumerable<Match>> GetPlayedMatches(int? roundId, string? tournamentName);
        Task<Player?> GetWinnerOfMatch(int matchId);
        Task<IEnumerable<Player?>?> GetAllWinnersOfRound(int roundId);
        Task<Match> RescheduleAMatch(int matchId, DateTime rescheduleDate);
        
    }
}