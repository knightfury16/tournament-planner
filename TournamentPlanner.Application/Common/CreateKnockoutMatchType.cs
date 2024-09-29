using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateKnockoutMatchType : ICreateMatchType
{


    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament,
    List<Player> players, string? prefix = null, List<int>? seederPlayerIds = null)
    {
        //im here, and i will be here only once in my tp life. according to Axiom.
        //there is no work here all the work will be done by scheduler of knockout match type
        prefix ??= "Knockout";
        var matchTypes = new List<MatchType>(){
            new KnockOut {
                Name = prefix,
                Players = players,
            }
        };

        return Task.FromResult((IEnumerable<MatchType>?)matchTypes);
    }
}
