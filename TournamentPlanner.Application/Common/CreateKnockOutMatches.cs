using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateKnockOutMatches : ICreateMatch
{
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType)
    {
        throw new NotImplementedException();
    }
}
