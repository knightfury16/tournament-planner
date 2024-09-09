using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.Common;

public class CreateKnockoutMatchType : ICreateMatchType
{

    public Task<IEnumerable<Domain.Entities.MatchType>?> CreateMatchType(Tournament tournament, string? prefix = "Knockout")
    {
        throw new NotImplementedException();
    }
}
