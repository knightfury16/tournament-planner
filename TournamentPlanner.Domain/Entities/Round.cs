using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Round: BaseEntity
    {
        public int RoundNumber { get; set; }
        public List<Match> Matches { get; set; } = new();
        public Tournament? Tournament { get; set; } = new();
        
    }
}