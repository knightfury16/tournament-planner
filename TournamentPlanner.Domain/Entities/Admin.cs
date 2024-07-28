using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentPlanner.Domain.Entities
{
    public class Admin : User
    {
        public required string PhoneNumber { get; set; }
    }
}