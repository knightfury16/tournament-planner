namespace TournamentPlanner.Application.DTOs;

public class SchedulingInfo
{
    public TimeSpan? EachMatchTime { get; set; } = TimeSpan.FromMinutes(30);
    public TimeOnly? StartTime { get; set; }
    public DateTime? StartDate { get; set; }
    
}
