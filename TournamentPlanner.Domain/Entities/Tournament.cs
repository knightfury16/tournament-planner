using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class Tournament<TScore> : BaseEntity where TScore : IScore
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

        public required GameType<TScore> GameType { get; set; }

        public List<Player> Participants { get; set; } = new();

        



        


    }
}