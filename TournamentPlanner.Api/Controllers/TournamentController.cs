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
        private readonly ITournamentUseCase _tournamentUseCase;

        public TournamentController(ITournamentUseCase tournamnetUseCase)
        {
            _tournamentUseCase = tournamnetUseCase;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Tournament))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTournament([FromBody] TournamentDto tournamentDto)
        {
            if (tournamentDto == null)
            {
                return BadRequest("Tournament information needed");
            }
            var tour = await _tournamentUseCase.AddTournamnet(tournamentDto);
            return CreatedAtAction(nameof(GetTournamentById), new {id = tour.Id}, tour);
        }

        [HttpGet("{id}", Name = nameof(GetTournamentById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tournament))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }
            var tournament = await _tournamentUseCase.GetTournamentbyId(id);
            return Ok(tournament);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTournament()
        {
            IEnumerable<Tournament> tournamets = await _tournamentUseCase.GetAll();
            return Ok(tournamets);
        }
    }
}
