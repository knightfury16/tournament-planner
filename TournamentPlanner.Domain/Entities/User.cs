using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    public abstract class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}