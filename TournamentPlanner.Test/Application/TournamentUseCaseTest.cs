namespace TournamentPlanner.Test.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Application.UseCases.TournamentUseCase;

public class TournamentUseCaseTest
{
    private readonly Tournament tournament;

    private readonly TournamentDto tournamentDto;
    private readonly Mock<IRepository<Tournament, Tournament>> tournamentRepositoryMock;
    private readonly TournamentUseCase tournamentUseCase;

    public TournamentUseCaseTest()
    {
        tournamentDto = new TournamentDto
        {
            Name = "Test tournament",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1)
        };

        tournament = new Tournament
        {
            Name = tournamentDto.Name,
            StartDate = tournamentDto.StartDate,
            EndDate = tournamentDto.EndDate,
        };

        tournamentRepositoryMock = new Mock<IRepository<Tournament, Tournament>>();

        tournamentUseCase = new TournamentUseCase(tournamentRepositoryMock.Object);

    }
    [Fact]
    public async Task AddTournament_Should_AddTournament()
    {
        // Arrange
        var tournamentDto = new TournamentDto { Name = "Test Tournament", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };

        tournamentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Tournament>())).Returns(Task.FromResult(tournament));

        tournamentRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await tournamentUseCase.AddTournamnet(tournamentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tournamentDto.Name, result.Name);
        Assert.Equal(tournamentDto.StartDate, result.StartDate);
        Assert.Equal(tournamentDto.EndDate, result.EndDate);

        tournamentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Tournament>()), Times.Once);
        tournamentRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_ReturnFilteredTournaments()
    {
        // Arrange
        var tournaments = new List<Tournament>
        {
            new Tournament { Name = "Test Tournament 1", StartDate = new DateTime(2023, 6, 1), EndDate = new DateTime(2023, 6, 2) },
            new Tournament { Name = "Test Tournament 2", StartDate = new DateTime(2023, 5, 30), EndDate = new DateTime(2023, 6, 3) }
        };


        tournamentRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<List<Func<Tournament, bool>>>())).ReturnsAsync(tournaments);

        // Act
        var result = await tournamentUseCase.GetAll("Test", null ,null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Test Tournament 1", result.First().Name);

    }

    [Fact]
    public async Task GetTournamentById_Should_ReturnTournament()
    {
        // Arrange
        var tournamentId = 1;
        var expectedTournament = new Tournament { Id = tournamentId, Name = "Test Tournament", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };

        tournamentRepositoryMock.Setup(x => x.GetByIdAsync(tournamentId)).ReturnsAsync(expectedTournament);

        // Act
        var result = await tournamentUseCase.GetTournamentbyId(tournamentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, tournamentId);
        Assert.Equal("Test Tournament", result.Name);
    }
}