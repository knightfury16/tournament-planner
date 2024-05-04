using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public class Player: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public Tournament Tournament { get; set; } = new();

        public int TournamentId { get; set; }

    }
}