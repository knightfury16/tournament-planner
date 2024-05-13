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
        private readonly ITournamentUseCase tournamentUseCase;

        public TournamentController(ITournamentUseCase tournamnetUseCase)
        {
            this.tournamentUseCase = tournamnetUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> AddTournament([FromBody] TournamentDto tournamentDto)
        {
            if (tournamentDto == null)
            {
                return BadRequest("Tournament information needed");
            }
            var tour = await tournamentUseCase.AddTournamnet(tournamentDto);
            return CreatedAtAction(nameof(GetTournamentById), new {name = tour.Name}, tour);
        }

        [HttpGet("{id}", Name = nameof(GetTournamentById))]
        public async Task<IActionResult> GetTournamentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }
            var tournament = await tournamentUseCase.GetTournamentbyId(id);
            return Ok(tournament);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTournament()
        {
            IEnumerable<Tournament> tournamets = await tournamentUseCase.GetAll();
            return Ok(tournamets);
        }
    }
}
