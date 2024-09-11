namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;


public class Match : BaseEntity
{
    public required Player FirstPlayer { get; set; }
    public int Player1Id { get; set; }

    public required Player SecondPlayer { get; set; }
    public int Player2Id { get; set; }

    public Player? Winner { get; set; }
    public int? WinnerId { get; set; }

    public required Tournament Tournament { get; set; }
    public int TournamentId { get; set; }

    public required Round Round { get; set; }
    public int RoundId { get; set; }

    public DateTime? GameScheduled { get; set; }

    public DateTime? GamePlayed { get; set; }

    public object? ScoreJson { get; set; }
    // Added to support match rescheduling
    public bool IsRescheduled { get; set; }
    public string? RescheduleReason { get; set; }
    public Admin? RescheduledBy { get; set; }
    public int? RescheduledById { get; set; }

    public TimeSpan Duration { get; set; }
    public string? CourtName { get; set; } 
    // public required GameFormat<Score> GameFormat { get; set; } // we handle the game format in runtime, no need to save

}
