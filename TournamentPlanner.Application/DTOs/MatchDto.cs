namespace TournamentPlanner.Application.DTOs;
public class MatchDto
{
    public required PlayerDto FirstPlayer { get; set; }

    public required PlayerDto SecondPlayer { get; set; }

    public PlayerDto? Winner { get; set; }
    //TODO: dont have matchtype in match any more
    //TODO: fix it
    // public int MatchTypeId { get; set; }

    public DateTime? GameScheduled { get; set; }

    public DateTime? GamePlayed { get; set; }

    public string? ScoreJson { get; set; }
    // Added to support match rescheduling
    public bool IsRescheduled { get; set; }
    public string? RescheduleReason { get; set; }

}