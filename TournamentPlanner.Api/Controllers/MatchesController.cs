using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.UseCases.MatchUseCase;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    //TODO: Document route parameter
    [ApiController, Route("api/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchUseCase _matchUseCase;

        public MatchesController(IMatchUseCase matchUseCase)
        {
            _matchUseCase = matchUseCase;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(IEnumerable<Match>))]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchUseCase.GetAllMatches(null);
            return Ok(matches);
        }

        [HttpGet]
        [Route("open")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        public async Task<IActionResult> GetAllOpenMatches()
        {
            var matches = await _matchUseCase.GetOpenMatches(null);
            return Ok(matches);
        }

        [HttpGet]
        [Route("{roundId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMatchesOfRound(int roundId)
        {
            if (roundId <= 0)
            {
                return BadRequest("Invalid RoundId. RoundId cant be null or negative");
            }
            var matches = await _matchUseCase.GetAllMatches(roundId);
            return Ok(matches);
        }

        [HttpGet]
        [Route("round/{roundId}/winner")]
        public async Task<IActionResult> GetAllWinnerOfRound(int roundId)
        {
            var players = await _matchUseCase.GetAllWinnersOfRound(roundId);
            return Ok(players);
        }

        [HttpGet]
        [Route("{matchId}/winner")]
        public async Task<IActionResult> GetWinnerOfMatch(int matchId)
        {
            var player = await _matchUseCase.GetWinnerOfMatch(matchId);
            return Ok(player);
        }


        [HttpPut]
        [Route("{matchId}/reschedule")]
        public async Task<IActionResult> RescheduleMatch(int matchId, [FromBody] string rescheduledDate)
        {
            //TODO: parse date properly and catch
            DateOnly.TryParse(rescheduledDate, out var date);
            var match = await _matchUseCase.RescheduleAMatch(matchId, date);

            return Ok(match);
        }


    }
}