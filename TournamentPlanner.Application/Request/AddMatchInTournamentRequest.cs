using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class AddMatchInTournamentRequest : IRequest<MatchDto>
{
    public AddMatchDto AddMatchDto { get; }
    public int TournamentId { get; }

    public AddMatchInTournamentRequest(AddMatchDto addMatchDto, int tournamentId)
    {
        AddMatchDto = addMatchDto;
        TournamentId = tournamentId;
    }

}
