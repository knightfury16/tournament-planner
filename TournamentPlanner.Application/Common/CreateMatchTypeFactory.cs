using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common;

public class CreateMatchTypeFactory : ICreateMatchTypeFactory
{
    public ICreateMatchType GetMatchTypeCreator(TournamentType tournamentType)
    {
        return tournamentType switch
        {
            TournamentType.GroupStage => new CreateGroupMatchType(),
            TournamentType.Knockout => new CreateKnockoutMatchType(),
            _ => throw new ArgumentException($"The following {tournamentType} is not supported"),
        };
    }
}
