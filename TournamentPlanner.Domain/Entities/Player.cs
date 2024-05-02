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
        public int PhoneNumber { get; set; }

        public string Email { get; set; } = string.Empty;
        
    }
}