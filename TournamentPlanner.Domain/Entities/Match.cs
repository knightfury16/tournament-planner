using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Match: BaseEntity
    {
        public Player FirstPlayer { get; set; } = new();
        public Player SecondPlayer { get; set; } = new();

        public bool IsComplete { get; set; }

        public Player? Winner { get; set; } 

        public DateTime? GameScheduled { get; set; }

        public DateTime? GamePlayed { get; set; }

        public Round? Round { get; set; }
        public int? RoundId { get; set; }

        
    }
}