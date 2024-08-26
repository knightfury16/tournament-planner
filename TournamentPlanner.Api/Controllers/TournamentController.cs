using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Api.Models;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/tournament")]
    public class TournamentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TournamentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Tournament))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddTournament([FromBody] TournamentDto tournamentDto)
        // {
        //     if (tournamentDto == null)
        //     {
        //         return BadRequest("Tournament information needed");
        //     }
        //     var tour = await _mediator.AddTournamnet(tournamentDto);
        //     return CreatedAtAction(nameof(GetTournamentById), new { id = tour.Id }, tour);
        // }

        // [HttpGet("{id}", Name = nameof(GetTournamentById))]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TournamentResponseDto))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> GetTournamentById(int id)
        // {
        //     if (id <= 0)
        //     {
        //         return BadRequest("Id can not be negative");
        //     }
        //     var tournament = await _mediator.GetTournamentbyId(id);
        //     if (tournament == null)
        //     {
        //         return NotFound();
        //     }
        //     var tournamentResponseDto = convertToResponseDto(tournament);
        //     return Ok(tournamentResponseDto);
        // }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Tournament>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournament([FromQuery] TournamentSearchParams searchParams)
        {
            var request = new GetTournamentRequest
            {
                Name = searchParams.Name,
                SearchCategory = searchParams.SearchCategory ?? TournamentSearchCategory.ThisWeek,
                Status = searchParams.Status,
                GameTypeSupported = searchParams.GameTypeSupported,
                StartDate = searchParams.StartDate,
                EndDate = searchParams.EndDate
            };

            var tournaments = await _mediator.Send(request);

            if (tournaments == null)
            {
                return BadRequest("Invalid search parameters or no tournaments found.");
            }

            return Ok(tournaments);
        }
    }
}
