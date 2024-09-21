using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICreateMatchTypeFactory
{
    //!!Need a creator with MatchType param
    public ICreateMatchType GetMatchTypeCreator(TournamentType tournamentType);
}
