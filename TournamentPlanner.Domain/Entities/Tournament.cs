using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        // TODO: do this through fluent api
        [JsonIgnore]
        public List<Round> Rounds { get; set; } = new();

        [JsonIgnore]
        public List<Player> Players { get; set; } = new();

    }
}