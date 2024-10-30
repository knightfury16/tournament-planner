using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetRoundsOfMatchTypeRequest : IRequest<IEnumerable<RoundDto>>
{

    public int MatchTypeId { get; }

    public GetRoundsOfMatchTypeRequest(int matchTypeId)
    {
        MatchTypeId = matchTypeId;
    }

}
