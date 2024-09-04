using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Api.Models;
using TournamentPlanner.Application;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Application.Request;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Tournament))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTournament([FromBody] AddTournamentDto addTournamentDto)
        {
            if (addTournamentDto == null)
            {
                return BadRequest("Tournament information needed");
            }
            var addTournamentRequest = new AddTournamentRequest(addTournamentDto);

            var tournamentDto = await _mediator.Send(addTournamentRequest);

            if (tournamentDto == null)
            {
                return BadRequest("Tournament creation failed");
            }

            return CreatedAtAction(nameof(GetTournamentById), new { id = tournamentDto.Id }, tournamentDto);
        }

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

        [HttpGet("{id}", Name = nameof(GetTournamentById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FullTournamentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }
            var getTournamentByIdRequest = new GetTournamentByIdRequest(id);
            var tournamentDto = await _mediator.Send(getTournamentByIdRequest);

            if (tournamentDto == null)
            {
                return NotFound();
            }
            return Ok(tournamentDto);
        }

        [HttpGet("{id}/players", Name = nameof(GetTournamentPlayers))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentPlayers(int id)
        {
            var getTournamentPlayersRequest = new GetTournamentPlayersRequest(id);
            var playersDto = await _mediator.Send(getTournamentPlayersRequest);
            return Ok(playersDto);
        }


        //getTournamentMatches(id)
        [HttpGet("{id}/matches", Name = nameof(GetTournamentMatches))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentMatches(int id)
        {
            var getTournamentMatchesRequest = new GetTournamentMatchesRequest(id);
            var matchesDto = await _mediator.Send(getTournamentMatchesRequest);
            return Ok(matchesDto);
        }

        //getTournamentMatchTypes(id)
    }
}
