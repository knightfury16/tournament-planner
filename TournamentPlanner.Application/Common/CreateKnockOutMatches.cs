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
}
