using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.GenerateUseCase
{
    public interface IGenerate
    {
        Task<IEnumerable<Player>> AddPlayerAutoToTournament(string TournamentName);
        Task<List<Match>> MakeRoaster<T>(T TournamentIdentifier);
        Task<List<Match>> SimulateMatches<T>(T TournamentIdentifier);
    }
}