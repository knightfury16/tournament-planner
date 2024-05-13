using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Application.UseCases.TournamentUseCase;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/tournamnet")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentUseCase tournamnetUseCase;

        public TournamentController(ITournamentUseCase tournamnetUseCase)
        {
            this.tournamnetUseCase = tournamnetUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> AddTournament([FromBody] TournamentDto tournamentDto)
        {
            var tour = await tournamnetUseCase.AddTournamnet(tournamentDto);
            return Ok(tour);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTournament()
        {
            IEnumerable<Tournament> tournamets = await tournamnetUseCase.GetAll();
            return Ok(tournamets);
        }
    }
}
