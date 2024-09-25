using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetMatchTypeRequest : IRequest<MatchTypeDto>
{
    public int MatchTypeId { get; }

    public GetMatchTypeRequest(int matchTypeId)
    {
        MatchTypeId = matchTypeId;
    }
}
