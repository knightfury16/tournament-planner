using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.UseCases.GenerateUseCase;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController, Route("/generate")]
    public class GenerateController: ControllerBase
    {
        private readonly IGenerate _generate;

        public GenerateController(IGenerate generate)
        {
            _generate = generate;
        }


        [HttpPost]
        [Route("{tournamentName}")]
        public async Task<IActionResult> AddPlayerToTournament(string tournamentName){
            var players = await _generate.AddPlayerAutoToTournament(tournamentName);
            return Ok(players);
        }

        [HttpPost]
        [Route("/roaster")]
        public async Task<IActionResult> MakeRoaster([FromQuery] string tournamentName){
            var matches = await _generate.MakeRoaster(tournamentName);
            if(matches is null)
            {
                return Ok("Can not make roaster, previous round not finish");
            }
            return Ok(matches);
        }

        [HttpPost]
        [Route("/simulate")]
        public async Task<IActionResult> SimulateMatch([FromQuery]string tournamentName, bool allMatch)
        {
            var matches = await _generate.SimulateMatches(tournamentName, allMatch);
            if(matches is null)
            {
                return Ok("No match is scheduled yet.Please schedule some match first");
            }
            return Ok(matches);
        }

        
    }
}