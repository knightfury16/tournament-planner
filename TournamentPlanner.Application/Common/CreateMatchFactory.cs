using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public interface ICreateMatchFactory
{
    ICreateMatch GetMatchCreator(MatchType matchType);
}
public class CreateMatchFactory : ICreateMatchFactory
{
    public ICreateMatch GetMatchCreator(MatchType matchType)
    {
        return matchType switch
        {
            //!! hold debugger here
           Group => new CreateRoundRobinMatches(),
           KnockOut => new CreateKnockOutMatches(),
           _ => throw new ArgumentException("Unsupported match type: " + matchType),
        };
    }
}
