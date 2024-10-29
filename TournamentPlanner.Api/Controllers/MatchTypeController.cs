using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace MyApp.Namespace
{
    [ApiController]
    [Route("api/match-type")]
    public class MatchTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchTypeController(IMediator mediator)
        {
            _mediator = mediator;

        }

        //That is the get the Group-A for example
        [HttpGet("{matchTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchTypeDto>))]
        public async Task<IActionResult> GetMatchType(int matchTypeId)
        {
            if (matchTypeId < 0) return BadRequest("Invalid Id");

            var getMatchTypeRequest = new GetMatchTypeRequest(matchTypeId);
            var matchTypeDto = await _mediator.Send(getMatchTypeRequest);

            return Ok(matchTypeDto);
        }

        [HttpGet("{matchTypeId}/group-standing")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerStanding>))]
        public async Task<IActionResult> GetGroupStanding(int matchTypeId)
        {
            if (matchTypeId < 0) return BadRequest("Invalid Id");

            var getGroupStandingRequest = new GetGroupStandingRequest(matchTypeId);
            var playerStandings = await _mediator.Send(getGroupStandingRequest);

            return Ok(playerStandings);
        }

    }
}
