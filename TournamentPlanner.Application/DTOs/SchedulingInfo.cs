namespace TournamentPlanner.Application.DTOs;

public class SchedulingInfo
{
    //TODO: make scheduling info non nullable
    public static TimeOnly DefaultStartTime => new TimeOnly(10, 0); //10am
    public static TimeOnly DefaultEndTime => new TimeOnly(17, 0); //5pm
    public static TimeSpan DefaultMatchDuration => TimeSpan.FromMinutes(30); //30min
    public string? EachMatchTime { get; set; } = DefaultMatchDuration.ToString();
    public string? StartTime { get; set; } = DefaultStartTime.ToString(); //will default to 10am
    public string? EndTime { get; set; } = DefaultEndTime.ToString();//will default to 5pm
    public DateTime? StartDate { get; set; }
    public int MatchPerDay { get; set; } = 15; //each with default 30min match
    public int ParallelMatchesPossible { get; set; } = 1;
    
}
