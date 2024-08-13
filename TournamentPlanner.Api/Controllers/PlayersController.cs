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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerDto>))]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayer([FromQuery] string? name)
        {
            var getAllPlayerRequest = new GetAllPlayerRequest(name);
            var playersDto = await _mediator.Send(getAllPlayerRequest);
            return Ok(playersDto);
        }

        [HttpGet("{id}", Name = nameof(GetPlayerById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlayerById([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }

            var getPlayerByIdRequest = new GetPlayerByIdRequest(id);
            var fullPlayerDto = await _mediator.Send(getPlayerByIdRequest);
            if(fullPlayerDto == null){
                return NotFound();
            }

            return Ok(fullPlayerDto);

        }



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