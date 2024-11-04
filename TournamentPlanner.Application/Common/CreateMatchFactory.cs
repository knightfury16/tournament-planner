using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;
//!Depricated
public interface ICreateMatchFactory
{
    ICreateMatch GetMatchCreator(MatchType matchType);
}
public class CreateMatchFactory : ICreateMatchFactory
{
    public ICreateMatch GetMatchCreator(MatchType matchType)
    {
        throw new NotImplementedException();
    }
}
