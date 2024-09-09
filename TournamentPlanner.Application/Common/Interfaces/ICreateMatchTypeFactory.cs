using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICreateMatchTypeFactory
{
    public ICreateMatchType GetMatchTypeCreator(TournamentType tournamentType);
}
