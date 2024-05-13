using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.TournamentUseCase
{
    public interface ITournamentUseCase
    {
        Task<Tournament> AddTournamnet(TournamentDto tournamentDto);
        Task<Tournament> GetTournamentbyId(int id);
        Task<IEnumerable<Tournament>> GetAll(string? name, DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Tournament>> GetTournamentsByDate(DateTime startDate, DateTime endDate);
    }
}
