using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class PlayersController : ControllerBase
    {
        public IPlayerUseCase _playerUseCase { get; }

        public PlayersController(IPlayerUseCase playerUseCase)
        {
            _playerUseCase = playerUseCase;
        }

        [HttpGet]
        [Route("players")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Player>))]
        public async Task<IActionResult> GetAllPlayer([FromQuery] string? name)
        {
            var players = await _playerUseCase.GetPlayersAsync(name);
            return Ok(players);
        }


        [HttpPost]
        [Route("players")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Player))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerDto playerDto){
            var player = await _playerUseCase.AddPlayerAsync(playerDto);
            //TODO: make return type createdAt
            return Ok(player);
        }

    }
}