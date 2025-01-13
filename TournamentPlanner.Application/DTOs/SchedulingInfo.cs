namespace TournamentPlanner.Application.DTOs;

public class SchedulingInfo
{
    //TODO: make scheduling info non nullable
    public string? EachMatchTime { get; set; }
    public string? StartTime { get; set; }
    public DateTime? StartDate { get; set; }
    public int MatchPerDay { get; set; } = 15; //each with default 30min match
    public int ParallelMatchesPossible { get; set; } = 1;
    
}
