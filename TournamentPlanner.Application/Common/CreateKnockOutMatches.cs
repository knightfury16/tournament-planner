using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateKnockOutMatches : ICreateMatch
{
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType)
    {
        //Todo: figure out a way to keep seeder player
        throw new NotImplementedException();
    }

    static int highestPowerof2(int N)
    {
        // if N is a power of two simply return it
        if ((N & (N - 1)) == 0)
            return N;
        var num = Convert.ToString(N, 2);
        var num2 = num.Length + 1;
        // else set only the most significant bit
        return (1 << ((Convert.ToString(N, 2).Length)));
    }
}
