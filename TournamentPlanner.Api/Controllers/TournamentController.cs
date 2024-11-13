using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Api.Models;
using TournamentPlanner.Application;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
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

        //- Add tournament
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

        //- Register player in tournament
        //TODO: need to verify if a valid player is registering the tournament or not
        [HttpPost("register", Name = nameof(RegisterPlayerInTournament))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterPlayerInTournament([FromBody] RegistrationInTournamentDto registrationDto)
        {
            if (registrationDto == null)
            {
                return BadRequest("Registration information is required");
            }

            var registerPlayerInTournamentRequest = new RegisterPlayerInTournamentRequest(registrationDto);

            var result = await _mediator.Send(registerPlayerInTournamentRequest);

            return result ? Ok("Player successfully registered for the tournament") : BadRequest("Player registration failed");
        }


        //- Make match type
        //! Experimental
        [HttpPost("{id}/make-match-type", Name = nameof(MakeMatchTypesOfTournament))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeMatchTypesOfTournament(int id)
        {
            if(id <= 0 )return BadRequest("Tournament Id invalid");

            var createMatchTypeRequest = new CreateMatchTypeRequest(id);

            var matchTypes = await _mediator.Send(createMatchTypeRequest);

            return Ok(matchTypes);
        }


        //- Make tournament draw
        [HttpPost("{id}/make-draw", Name = nameof(MakeTournamentDraw))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeTournamentDraw(int id, [FromBody] MakeTournamentDrawDto drawDto)
        {

            var createTournamentDrawRequest = new CreateTournamentDrawRequest(id, drawDto.SeedersId, drawDto.MatchTypePrefix);

            var drawDtos = await _mediator.Send(createTournamentDrawRequest);

            return Ok(drawDtos);
        }

        //- Make tournament schedule
        [HttpPost("{id}/make-schedule", Name = nameof(MakeTournamentSchedule))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeTournamentSchedule(int id)
        {
            var schedulingInfo = new SchedulingInfo
            {
            };

            var createTournamentDrawRequest = new MakeTournamentMatchScheduleRequest(id, schedulingInfo);

            var drawDtos = await _mediator.Send(createTournamentDrawRequest);

            return Ok(drawDtos);
        }

        //- Change tournament status
        [HttpPost("{id}/change-status", Name = nameof(ChangeTournamentStatus))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeTournamentStatus(int id, [FromBody] TournamentStatus tournamentStatus)
        {
            if (id <= 0) return BadRequest("Tournament Id invalid");

            if (!Enum.IsDefined(typeof(TournamentStatus), tournamentStatus))
            {
                return BadRequest("Invalid tournament status");
            }

            var request = new ChangeTournamentStatusRequest(tournamentStatus, id);

            var success = await _mediator.Send(request);

            if (!success)
            {
                return BadRequest("Could not change tournament status");
            }

            return Ok("Tournament status changed successfully");
        }




        //! On maintenance
        // [HttpPost("{tournamentId}/add-match")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddTournamentMatch(int tournamentId, [FromBody] AddMatchDto addMatchDto)
        // {
        //     if (addMatchDto == null)
        //     {
        //         return BadRequest("Match to add information is required");
        //     }

        //     var addMatchToTournament = new AddMatchInTournamentRequest(addMatchDto, tournamentId);

        //     var matchDto = await _mediator.Send(addMatchToTournament);

        //     if (matchDto == null)
        //     {
        //         return BadRequest("Could not add match");
        //     }

        //     //TODO: make it created at action after making the get match by id route
        //     return Ok(matchDto);
        // }

        //- Get tournament
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

        //- Get tournament by id
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

        //- Get tournament players
        [HttpGet("{id}/players", Name = nameof(GetTournamentPlayers))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentPlayers(int id)
        {
            var getTournamentPlayersRequest = new GetTournamentPlayersRequest(id);
            var playersDto = await _mediator.Send(getTournamentPlayersRequest);
            return Ok(playersDto);
        }


        //- Get tournament matches
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
        //- Get tournament match type
        [HttpGet("{id}/match-types", Name = nameof(GetTournamentMatchTypes))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchTypeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentMatchTypes(int id)
        {
            var getTournamentMatchTypesRequest = new GetTournamentMatchTypesRequest(id);
            var matchTypesDto = await _mediator.Send(getTournamentMatchTypesRequest);
            return Ok(matchTypesDto);
        }

        //- Get tournament Draws
        [HttpGet("{id}/get-draws", Name = nameof(GetTournamentDraws))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DrawDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournamentDraws(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }
            var getTournamentDrawRequest = new GetTournamentDrawRequest(id);
            var draws = await _mediator.Send(getTournamentDrawRequest);

            if (draws == null)
            {
                return NotFound();
            }
            return Ok(draws);
        }
    }
}
