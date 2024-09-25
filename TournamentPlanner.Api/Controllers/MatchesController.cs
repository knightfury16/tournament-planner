using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/matches")] 
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchDto>))]
        public async Task<IActionResult> GetMatch(int matchId)
        {
            if(matchId < 0) return BadRequest("Invalid Id");

            var getMatchRequest = new GetMatchRequest(matchId);
            var matchDto = await _mediator.Send(getMatchRequest);

            return Ok(matchDto);
        }

        //TODO: Only admins or the admin that created the T can entry-match-score
        //TODO: Or we can have Co-Admin under the Admin that created the T and give them permission to do the score entry
        [HttpPost("{matchId}/entry-match-score")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EntryMatchScore(int matchId, [FromBody] AddMatchScoreDto addMatchScoreDto)
        {
            if (addMatchScoreDto == null)
            {
                return BadRequest("Need Score to update");
            }

            var addMatchScoreRequest = new AddMatchScoreRequest(matchId, addMatchScoreDto);

            var matchDto = await _mediator.Send(addMatchScoreRequest);

            if (matchDto == null)
            {
                return BadRequest("Could not entry match score");
            }

            return Ok(matchDto);
        }

        // [HttpGet]
        // [Route("open")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        // public async Task<IActionResult> GetAllOpenMatches([FromQuery] int? roundId, [FromQuery] int? tournamentId)
        // {
        //     var matches = await _mediator.GetOpenMatches(roundId, tournamentId);
        //     return Ok(matches);
        // }

        // [HttpGet]
        // [Route("played")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        // public async Task<IActionResult> GetPlayedMatches([FromQuery] int? roundId, [FromQuery] int? tournamentId)
        // {
        //     var matches = await _mediator.GetPlayedMatches(roundId, tournamentId);
        //     return Ok(matches);
        // }


        // [HttpGet]
        // [Route("{roundId}")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> GetAllMatchesOfRound(int roundId)
        // {
        //     if (roundId <= 0)
        //     {
        //         return BadRequest("Invalid RoundId. RoundId cant be null or negative");
        //     }
        //     var matches = await _mediator.GetAllRoundMatches(roundId);
        //     return Ok(matches);
        // }

        // [HttpGet]
        // [Route("round/{roundId}/winner")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Player>))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> GetAllWinnerOfRound(int roundId)
        // {
        //     if (roundId <= 0)
        //     {
        //         return BadRequest("Invalid RoundId. RoundId cant be null or negative");
        //     }
        //     var players = await _mediator.GetAllWinnersOfRound(roundId);
        //     return Ok(players);
        // }

        // [HttpGet]
        // [Route("{matchId}/winner")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> GetWinnerOfMatch(int matchId)
        // {
        //     if (matchId <= 0)
        //     {
        //         return BadRequest("Invalid MatchId. MatchId cant be null or negative");
        //     }

        //     var player = await _mediator.GetWinnerOfMatch(matchId);
        //     if (player == null)
        //     {
        //         return NotFound("Match not completed yet");
        //     }
        //     return Ok(player);
        // }


        // [HttpPut]
        // [Route("{matchId}/reschedule")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Match))]
        // public async Task<IActionResult> RescheduleMatch(int matchId, [FromBody] string rescheduledDate)
        // {
        //     //TODO: parse date properly and catch
        //     DateTime.TryParse(rescheduledDate, out var date);
        //     var match = await _mediator.RescheduleAMatch(matchId, date);

        //     return Ok(match);
        // }


    }
}