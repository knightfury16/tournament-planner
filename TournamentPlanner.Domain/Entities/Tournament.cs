using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Domain.Entities
{
    public class Tournament: BaseEntity
    {
        public required string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? RegistrationLastDate { get; set; }

        public int MaxParticipant { get; set; }

        public string? Venue { get; set; }

        public decimal RegistrationFee { get; set; }

        public int MinimumAgeOfRegistration { get; set; }

        public int WinnerPerGroup { get; set; }
        public int KnockOutStartNumber { get; set; }

        public ResolutionStrategy ParticipantResolutionStrategy { get; set; }

        public required TournamentType TournamentType { get; set; }

        public required GameType GameType { get; set; }

        public List<Player> Participants { get; set; } = new();

        public List<Group> Groups { get; set; } = new ();
        public List<KnockOut> KnockOuts { get; set; } = new ();
        public List<Match> Matches { get; set; } = new List<Match>();

        // Added to support tournament status
        public TournamentStatus Status { get; set; }
        // Added to support search functionality
        public bool IsSearchable => Status != TournamentStatus.Draft;

    }
}