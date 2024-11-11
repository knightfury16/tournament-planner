using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Test.Application.Common;

public class CreateRoundRobinMatchesTest
{
    private readonly CreateRoundRobinMatches _createRoundRobinMatches;
    private readonly Tournament _tournament;
    private readonly MatchType _matchType;

    public CreateRoundRobinMatchesTest()
    {
        _createRoundRobinMatches = new CreateRoundRobinMatches();
        _tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh();
        _matchType = new Group { Name = "Test Group A", Players = _tournament.Participants.ToList() };
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_EvenPlayers_CreatesCorrectMatches()
    {
        // Arrange
        var players = PlayerFixtures.GetSamplePlayers(4); // 4 players for even number of matches
        _matchType.Players = players;

        // Act
        var matches = await _createRoundRobinMatches.CreateMatches(_tournament, _matchType);

        // Assert
        Assert.Equal(6, matches.Count()); // 3 rounds * 2 matches per round = 6 matches 
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 1)); // 2 matches in the first round
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 2)); // 2 matches in the second round
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 3)); // 2 matches in the third round
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_OddPlayers_AddsByePlayerAndCreatesCorrectMatches()
    {
        // Arrange
        var players = PlayerFixtures.GetSamplePlayers(5); // 5 players for odd number of matches
        var playerCountBeforeBye = players.Count();
        _matchType.Players = players;

        // Act
        var matches = await _createRoundRobinMatches.CreateMatches(_tournament, _matchType);

        // Assert
        Assert.Equal(10, matches.Count()); //by calculation its 15 matches, not making matches for bye so -5 gets 10
        Assert.Equal(playerCountBeforeBye + 1, players.Count);// CreateMmatches() added 1 bye player
        Assert.Contains(Utility.ByePlayerName, _matchType.Players.Select(p => p.Name));
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 1)); // 2 matches in the first round (not making matches for bye)
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 2)); // 2 matches in the second round
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 3)); // 2 matches in the third round
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 4)); // 2 matches in the fourth round
        Assert.Equal(2, matches.Count(m => m.Round.RoundNumber == 5)); // 2 matches in the fifth round
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_SinglePlayer_ReturnsNoMatches()
    {
        // Arrange
        var players = PlayerFixtures.GetSamplePlayers(1); // 1 player
        _matchType.Players = players;

        // Act
        var matches = await _createRoundRobinMatches.CreateMatches(_tournament, _matchType);

        // Assert
        Assert.Empty(matches); //single player get bye and not making matches for bye
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_NoPlayers_ReturnsNoMatches()
    {
        // Arrange
        _matchType.Players = new List<Player>(); // No players

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(async () => await _createRoundRobinMatches.CreateMatches(_tournament, _matchType)); // No players found in group to create matches
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_NonGroupMatchType_ThrowsException()
    {
        // Arrange
        var players = PlayerFixtures.GetSamplePlayers(4); // 4 players for a valid match
        var knockOutMatchType = new KnockOut{Name = "Knockout Matchtype"};
        knockOutMatchType.Players = players;

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await _createRoundRobinMatches.CreateMatches(_tournament, knockOutMatchType));
    }

    [Fact]
    public async Task TestCreateRoundRobinMatches_EveryPlayerPlaysEveryone()
    {
        // Arrange
        var players = PlayerFixtures.GetSamplePlayers(4); // 4 players for a valid match
        _matchType.Players = players;

        // Act
        var matches = await _createRoundRobinMatches.CreateMatches(_tournament, _matchType);

        // Assert
        foreach (var player in players)
        {
            var opponents = matches.Where(m => m.FirstPlayer == player || m.SecondPlayer == player)
                                    .Select(m => m.FirstPlayer == player ? m.SecondPlayer : m.FirstPlayer)
                                    .ToList();
            Assert.Equal(players.Count() - 1, opponents.Count()); // Each player plays every other player
            Assert.All(opponents, opponent => Assert.Contains(opponent, players));
        }
    }
}

