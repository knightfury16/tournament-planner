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

        // TODO: do this through fluent api
        [JsonIgnore]
        public List<Match> Matches { get; set; } = new();
        public Tournament? Tournament { get; set; } = new();
        public int TournamentId { get; set; }
        
    }
}