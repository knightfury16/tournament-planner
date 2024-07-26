using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.DTOs
{
    //had to make this for the plater count
    public class TournamentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int PlayerCount { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}