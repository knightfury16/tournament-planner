using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/players")]
    public class PlayersController : ControllerBase
    {
        public IMediator _mediator { get; }

        public PlayersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Player>))]
        public async Task<IActionResult> GetAllPlayer([FromQuery] string? name)
        {
            var getAllPlayerRequest = new GetAllPlayerRequest();
            var players = await _mediator.Send(getAllPlayerRequest);
            return Ok(players);
        }

        // [HttpGet("{id}", Name = nameof(GetPlayerById))]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> GetPlayerById([FromRoute] int id)
        // {

        //     if (id <= 0)
        //     {
        //         return BadRequest("Id can not be negative");
        //     }
        //     var player = await _mediator.GetPlayerById(id);

        //     if (player == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(player);

        // }


        // [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Player))]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddPlayer([FromBody] PlayerDto playerDto)
        // {
        //     var player = await _mediator.AddPlayerAsync(playerDto);
        //     return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        // }

    }
}