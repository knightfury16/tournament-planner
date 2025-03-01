using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TournamentPlanner.Application.Common.Attributes;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.DTOs;

//* Finding: Property are being validated first then the class validator or DateRangeValidator work. Propagating up
[FeasibilityValidator]
[DateRangeValidator]
public class AddTournamentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Name should at least 5 character long")]
    [MaxLength(100, ErrorMessage = "Name should at most 100 charecter long")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "You must specify start date")]
    [Future]
    public required DateTime StartDate { get; set; }

    [Required(ErrorMessage = "You must specify end date")]
    public required DateTime EndDate { get; set; }

    [Required]
    public required GameTypeDto GameType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TournamentStatus Status { get; set; } = TournamentStatus.Draft;

    //TODO: make a validator or extends the current validator to check for Registration last date
    public DateTime? RegistrationLastDate { get; set; }

    // In theory the Feasibility Validator will take care of anything unsual.
    public int MaxParticipant { get; set; } = 64; // This is the max value for knockout tournament type. Putting here just in case.
    public string? Venue { get; set; }
    public decimal RegistrationFee { get; set; }
    public int MinimumAgeOfRegistration { get; set; }

    public int WinnerPerGroup { get; set; } = 2; // Default value 2. Will not let the User choose. This is enforced by me.

    [Range(2, 64, ErrorMessage = "Knockout start number should be between 2 to 64")]
    [PowerOfTwo] // I am being fancy here, could just make it enum and only allow those values
    public int KnockOutStartNumber { get; set; } = 16; // Default value 16
    public ResolutionStrategy? ParticipantResolutionStrategy { get; set; } =
        ResolutionStrategy.StatBased; // Default value StatBased

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TournamentType? TournamentType { get; set; } =
        Domain.Enum.TournamentType.GroupStage; // Default value GroupStage
}
