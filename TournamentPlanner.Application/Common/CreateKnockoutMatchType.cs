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

        var knockoutMatchType = new KnockOut
        {
            Name = prefix,
            Players = players
        };

        if (seederPlayerIds != null)
        {
            knockoutMatchType.SeededPlayers = players.Where(p => seederPlayerIds.Any(id => id == p.Id)).Select(p => new SeededPlayer { Player = p, MatchType = knockoutMatchType }).ToList();
        }



        var matchTypes = new List<MatchType>(){
            knockoutMatchType
        };

        return Task.FromResult((IEnumerable<MatchType>?)matchTypes);
    }
}
