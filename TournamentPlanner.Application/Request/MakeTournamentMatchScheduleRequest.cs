using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class MakeTournamentMatchScheduleRequest : IRequest<IEnumerable<MatchDto>>
{

    public int TournamentId { get; set; }
    public SchedulingInfo SchedulingInfo { get; set; }
    public MakeTournamentMatchScheduleRequest(int tournamentId, SchedulingInfo schedulingInfo)
    {
        TournamentId = tournamentId;
        SchedulingInfo = schedulingInfo;
    }

}