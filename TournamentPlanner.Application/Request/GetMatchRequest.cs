using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request;

public class GetMatchRequest : IRequest<MatchDto>
{
    public int MatchId { get; }
    
    public GetMatchRequest(int matchId)
    {
        MatchId = matchId;
    }

}
