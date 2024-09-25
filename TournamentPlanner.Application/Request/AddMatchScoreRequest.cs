using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request;

public class AddMatchScoreRequest : IRequest<MatchDto>
{
    public int MatchId { get; }
    public AddMatchScoreDto AddMatchScoreDto { get; }
    public AddMatchScoreRequest(int matchId, AddMatchScoreDto addMatchScoreDto)
    {
        MatchId = matchId;
        AddMatchScoreDto = addMatchScoreDto;
    }
}
