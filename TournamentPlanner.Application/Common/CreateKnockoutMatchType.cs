using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.Common;

public class CreateKnockoutMatchType : ICreateMatchType
{


    public Task<IEnumerable<Domain.Entities.MatchType>?> CreateMatchType(Tournament tournament, string? prefix = null, List<int>? seederPlayerIds = null)
    {
        throw new NotImplementedException();
    }
}
