using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.UseCases.GenerateUseCase;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController, Route("api/generate")]
    public class GenerateController: ControllerBase
    {
        private readonly IGenerate _generate;

        public GenerateController(IGenerate generate)
        {
            _generate = generate;
        }


        [HttpPost]
        [Route("{tournamentName}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(IEnumerable<Player>))]
        public async Task<IActionResult> AddPlayerToTournament(string tournamentName){
            var players = await _generate.AddTournamentAndPlayerAuto(tournamentName);
            return Ok(players);
        }

        [HttpPost]
        [Route("roaster/{tournamentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeRoaster([FromRoute] int tournamentId){
            var matches = await _generate.MakeRoaster(tournamentId);
            if(matches is null)
            {
                return BadRequest("Can not make roaster, previous round not finish");
            }
            return Ok(matches);
        }

        [HttpPost]
        [Route("simulate/{tournamentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Match>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SimulateMatch([FromRoute]int tournamentId, [FromQuery] bool allMatch)
        {
            var matches = await _generate.SimulateMatches(tournamentId, allMatch);
            if(matches is null)
            {
                return BadRequest("No match is scheduled yet.Please schedule some match first");
            }
            return Ok(matches);
        }
        
        //Make a controller to change Round StartDate
        //Change Round Start Date(int RoundID, DateTime)
    }
}