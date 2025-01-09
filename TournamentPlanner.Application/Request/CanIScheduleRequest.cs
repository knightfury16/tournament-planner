using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CanIScheduleRequest : IRequest<CanIScheduleDto>
{
    public int TournamentId { get; set; }

    public CanIScheduleRequest(int tournamentId)
    {
        TournamentId = tournamentId;
    }
}
