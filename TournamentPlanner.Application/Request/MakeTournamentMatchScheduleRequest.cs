using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class MakeTournamentMatchScheduleRequest : IRequest<IEnumerable<MatchDto>>
{

    public int TournamentId { get; set; }
    public TimeSpan? EachMatchTime { get; set; } = TimeSpan.FromMinutes(30); // default 30 minute
    public TimeOnly? StartTime { get; set; }

    public MakeTournamentMatchScheduleRequest(int tournamentId, TimeSpan? eachMatchTime, TimeOnly? startTime)
    {
        TournamentId = tournamentId;
        EachMatchTime = eachMatchTime;
        StartTime = startTime;
    }

}