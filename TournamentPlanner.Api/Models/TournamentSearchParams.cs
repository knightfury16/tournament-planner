using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Api.Models
{
    public class TournamentSearchParams
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "searchCategory")]
        public TournamentSearchCategory? SearchCategory { get; set; }

        [FromQuery(Name = "status")]
        public TournamentStatus? Status { get; set; }

        [FromQuery(Name = "gameType")]
        public GameTypeSupported? GameTypeSupported { get; set; }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}