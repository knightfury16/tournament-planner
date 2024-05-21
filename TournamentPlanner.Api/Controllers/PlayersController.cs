using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/players")]
    public class PlayersController : ControllerBase
    {
        public IPlayerUseCase _playerUseCase { get; }

        public PlayersController(IPlayerUseCase playerUseCase)
        {
            _playerUseCase = playerUseCase;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Player>))]
        public async Task<IActionResult> GetAllPlayer([FromQuery] string? name)
        {
            var players = await _playerUseCase.GetPlayersAsync(name);
            return Ok(players);
        }

        [HttpGet("{id}", Name = nameof(GetPlayerById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlayerById([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id can not be negative");
                }
                var player = await _playerUseCase.GetPlayerById(id);
                return Ok(player);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("not found"))
                {
                    return NotFound(e.Message);
                }
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Player))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerDto playerDto)
        {
            var player = await _playerUseCase.AddPlayerAsync(playerDto);
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        }

    }
}