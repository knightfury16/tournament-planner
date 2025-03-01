namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Enum;

public class Tournament : BaseEntity
{
    public required string Name { get; set; }
    public required Admin CreatedBy { get; set; }
    public int AdminId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    public DateTime? RegistrationLastDate { get; set; }

    public int MaxParticipant { get; set; } // Max participants depends on the Tournament Type now.
    public string? Venue { get; set; }

    public decimal RegistrationFee { get; set; }

    public int MinimumAgeOfRegistration { get; set; }

    public int WinnerPerGroup { get; set; } // Default value 2
    public int KnockOutStartNumber { get; set; } // Default value 16

    public ResolutionStrategy? ParticipantResolutionStrategy { get; set; } // Default value StatBased

    public required TournamentType? TournamentType { get; set; } // Default value GroupStage

    //setting the inital current state during creation of tournament in datacontext, based on tournament type
    public TournamentState CurrentState { get; set; }

    public required GameType GameType { get; set; }
    public int GameTypeId { get; set; }

    public List<Player> Participants { get; set; } = new();
    public List<Draw> Draws { get; set; } = new();
    public List<Match> Matches { get; set; } = new();

    // Added to support tournament status
    public TournamentStatus? Status { get; set; } // Default value is Draft

    // Added to support search functionality
    public bool IsSearchable => Status != TournamentStatus.Draft; //TODO: If not searchable dont include it in the query
}
