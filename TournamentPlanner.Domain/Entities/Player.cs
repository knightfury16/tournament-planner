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


        //! Player should be independent of the Tournament.
        public Tournament? Tournament { get; set; }

        public int? TournamentId { get; set; }

    }
}