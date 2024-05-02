using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Match: BaseEntity
    {
        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

        public bool IsComplete { get; set; }

        public Player? Winner { get; set; }

        public DateOnly? GameScheduled { get; set; }

        public DateOnly? GamePlayed { get; set; }
        
    }
}