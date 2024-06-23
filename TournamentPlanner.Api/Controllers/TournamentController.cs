using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Application.UseCases.TournamentUseCase;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/tournament")]
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
            return CreatedAtAction(nameof(GetTournamentById), new { id = tour.Id }, tour);
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
            if (tournament == null)
            {
                return NotFound();
            }
            return Ok(tournament);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Tournament>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournament([FromQuery] string? name, [FromQuery] string? start, [FromQuery] string? end)
        {
            DateOnly? startDate = string.IsNullOrEmpty(start) ? null : DateOnly.Parse(start);

            DateOnly? endDate = string.IsNullOrEmpty(end) ? null : DateOnly.Parse(end);


            if (endDate != null && endDate <= startDate)
            {
                return BadRequest("Invalid Date.Please Enter a valid date range");
            }

            IEnumerable<Tournament> tournamets = await _tournamentUseCase.GetAll(name, startDate, endDate);
            return Ok(tournamets);
        }
    }
}
