    // GetAllPlayer returns a list of players when name is null
namespace Api.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentPlanner.Api.Controllers;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Domain.Entities;
using Xunit;

public class PlayersControllerTests
{
    [Fact]
    public async Task get_all_player_returns_list_of_players_when_name_is_null()
    {
        // Arrange
        var mockPlayerUseCase = new Mock<IPlayerUseCase>();
        mockPlayerUseCase.Setup(useCase => useCase.GetPlayersAsync(null))
                         .ReturnsAsync(new List<Player> { new Player { Id = 1, Name = "John Doe" } });

        var controller = new PlayersController(mockPlayerUseCase.Object);

        // Act
        var result = await controller.GetAllPlayer(null) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var players = Assert.IsAssignableFrom<IEnumerable<Player>>(result.Value);
        Assert.Single(players);
    }

     [Fact]
    public async Task get_player_by_id_returns_400_when_id_is_negative()
    {
        // Arrange
        var mockPlayerUseCase = new Mock<IPlayerUseCase>();
        var controller = new PlayersController(mockPlayerUseCase.Object);

        // Act
        var result = await controller.GetPlayerById(-1) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.Equal("Id can not be negative", result.Value);
    }
}