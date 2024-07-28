using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Round: BaseEntity
    {
        public int RoundNumber { get; set; }

        public DateTime? StartTime { get; set; }

    }
}