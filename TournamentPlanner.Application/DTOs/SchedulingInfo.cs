namespace TournamentPlanner.Application.DTOs;

public class SchedulingInfo
{
    //TODO: make scheduling info non nullable
    public TimeSpan? EachMatchTime { get; set; } = TimeSpan.FromMinutes(30);
    public TimeOnly? StartTime { get; set; }
    public DateTime? StartDate { get; set; }
    
}
