using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.DTOs;

public class AddTournamentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Name should at least 5 character long")]
    [MaxLength(100, ErrorMessage = "Name should at most 100 charecter long")]
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required GameTypeDto GameType { get; set; }
    public TournamentStatus Status { get; set; }
    public DateTime? RegistrationLastDate { get; set; }
    public int MaxParticipant { get; set; }
    public string? Venue { get; set; }
    public decimal RegistrationFee { get; set; }
    public int MinimumAgeOfRegistration { get; set; }
    public int WinnerPerGroup { get; set; } // Default value 2
    public int KnockOutStartNumber { get; set; } // Default value 16
    public ResolutionStrategy? ParticipantResolutionStrategy { get; set; } // Default value StatBased
    public required TournamentType? TournamentType { get; set; } // Default value GroupStage

}