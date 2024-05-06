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
            return Ok(matches);
        }

        
    }
}