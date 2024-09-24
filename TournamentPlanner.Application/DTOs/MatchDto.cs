namespace TournamentPlanner.Application.DTOs;
public class MatchDto
{
    public int Id { get; set; }
    public required PlayerDto FirstPlayer { get; set; }

    public required PlayerDto SecondPlayer { get; set; }

    public PlayerDto? Winner { get; set; }
    public int RoundId { get; set; }
    public TimeSpan Duration { get; set; }

    public DateTime? GameScheduled { get; set; }

    public DateTime? GamePlayed { get; set; }

    public string? ScoreJson { get; set; }
    // Added to support match rescheduling
    public bool IsRescheduled { get; set; }
    public string? RescheduleReason { get; set; }

}