using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.GenerateUseCase
{
    public interface IGenerate
    {
        Task<IEnumerable<Player>> AddTournamentAndPlayerAuto(string tournamentName);
        Task<List<Match>?> MakeRoaster(int tournamentId);
        Task<List<Match>?> SimulateMatches(int tournamentId, bool allMatch);
    }
}