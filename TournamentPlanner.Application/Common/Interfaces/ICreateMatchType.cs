using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICreateMatchType
{
    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, string? prefix = null);
}
