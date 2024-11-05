namespace TournamentPlanner.Test.Application.Common;

using Xunit;
using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Common;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Test.Fixtures;
using System.Threading.Tasks;
using System.Linq;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain;

public class CreateKnockOutMatchesTest
{
    private readonly Mock<IRepository<MatchType>> _matchTypeRepositoryMock;
    private readonly Mock<IRepository<Round>> _roundRepositoryMock;
    private readonly CreateKnockOutMatches _createKnockOutMatches;

    public CreateKnockOutMatchesTest()
    {
        _matchTypeRepositoryMock = new Mock<IRepository<MatchType>>();
        _roundRepositoryMock = new Mock<IRepository<Round>>();
        _createKnockOutMatches = new CreateKnockOutMatches(_matchTypeRepositoryMock.Object, _roundRepositoryMock.Object);
    }
    //Test correact number of matches with player power of 2
    [Fact]
    public async Task CreateFirstRoundMatches_WithPlayerPowerOfTwo_ReturnMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);
        tournament.Participants.AddRange(players);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.CompletedTask); //No seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(4, matches.Count());

        // Arrange
        players = PlayerFixtures.GetSamplePlayers(64);
        tournament.Participants.AddRange(players);
        matchType = new KnockOut { Name = "Kncockout test", Players = players };
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.CompletedTask); //No seed

        // Act
        matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(32, matches.Count());
    }
    //Test correct number of matches with player not power of 2
    [Fact]
    public async Task CreateFirstRoundMatches_WithPlayerUneven_ReturnMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(23);
        tournament.Participants.AddRange(players);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.CompletedTask); //No seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(16, matches.Count());

        // Arrange
        players = PlayerFixtures.GetSamplePlayers(66);
        tournament.Participants.AddRange(players);
        matchType = new KnockOut { Name = "Kncockout test", Players = players };
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.CompletedTask); //No seed

        // Act
        matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(64, matches.Count());
    }


    //Create first round with seeder players, return correct number of matches
    [Fact]
    public async Task CreateFirstRoundMatches_WithSeededPlayers_ReturnsCorrectMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        tournament.Participants.AddRange(players);
        var seederPlayers = new List<SeededPlayer> { new SeededPlayer { Player = players[0], MatchType = matchType }, new SeededPlayer { Player = players[1], MatchType = matchType } };
        matchType.SeededPlayers.AddRange(seederPlayers);
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.FromResult(seederPlayers)); //Seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(4, matches.Count()); //correct number of matches
    }


    //Create first round matches with seeder players but number of player perfect power of 2 return 0 number of bye matches
    [Fact]
    public async Task CreateFirstRoundMatches_WithSeededAndPerfectPowerOfTwoPlayers_ReturnsCorrectByes()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);
        tournament.Participants.AddRange(players);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        var seederPlayers = new List<SeededPlayer> { new SeededPlayer { Player = players[0], MatchType = matchType }, new SeededPlayer { Player = players[1], MatchType = matchType } };
        matchType.SeededPlayers.AddRange(seederPlayers);
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.FromResult(seederPlayers)); //Seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(0, matches.Count(m => m.FirstPlayer.Name == Utility.ByePlayerName || m.SecondPlayer.Name == Utility.ByePlayerName)); //no one get bye as the number of player is perfect power of 2
    }

    //Create first round matches with seeder players retruns correct number of bye matches
    [Fact]
    public async Task CreateFirstRoundMatches_WithSeededPlayers_ReturnsCorrectByes()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(13);
        tournament.Participants.AddRange(players);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        var seederPlayers = new List<SeededPlayer> { new SeededPlayer { Player = players[0], MatchType = matchType }, new SeededPlayer { Player = players[1], MatchType = matchType } };
        matchType.SeededPlayers.AddRange(seederPlayers);
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.FromResult(seederPlayers)); //Seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(3, matches.Count(m => m.FirstPlayer.Name == Utility.ByePlayerName || m.SecondPlayer.Name == Utility.ByePlayerName));
        //bye player is always the first player
        Assert.Equal(2, matches.Count(m => m.FirstPlayer.Name == Utility.ByePlayerName && seederPlayers.Any(sp => sp.Player == m.SecondPlayer))); //bye matches contain players from the seederPlayes list
    }

    [Fact]
    public async Task CreateFirstRoundMatches_WithoutSeededPlayers_ReturnsCorrectByes()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(13);
        tournament.Participants.AddRange(players);
        var matchType = new KnockOut { Name = "Kncockout test", Players = players };
        _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers)).Returns(Task.CompletedTask); //No seed

        // Act
        var matches = await _createKnockOutMatches.CreateFirstRoundMatches(tournament, matchType);

        // Assert
        Assert.NotNull(matches);
        Assert.Equal(3, matches.Count(m => m.FirstPlayer.Name == Utility.ByePlayerName || m.SecondPlayer.Name == Utility.ByePlayerName));
    }


        [Fact]
        public async Task CreateFirstRoundMatchesAfterGroup_WithEvenGroupOfPlayerStanding_ReturnsMatches()
        {
            // Arrange
            var tournament = TournamentFixtures.GetKnockoutTournament();
            var players = PlayerFixtures.GetSamplePlayers(8).ToList();
            var matchType = new KnockOut { Name = "Knockout Test", Players = players};

            var groupAStanding = new List<PlayerStanding> 
            {
                new PlayerStanding { Player = players[0], GamesWon = 4, GamesLost = 1, Ranking = 1},
                new PlayerStanding { Player = players[1], GamesWon = 3, GamesLost = 1, Ranking = 2},
            };
            var groupBStanding = new List<PlayerStanding> 
            {

                new PlayerStanding { Player = players[2], GamesWon = 3, GamesLost = 2, Ranking = 1},
                new PlayerStanding { Player = players[3], GamesWon = 2, GamesLost = 3, Ranking = 2},
            };
            var groupCStanding = new List<PlayerStanding>
            {

                new PlayerStanding { Player = players[4], GamesWon = 4, GamesLost = 1, Ranking = 1},
                new PlayerStanding { Player = players[5], GamesWon = 3, GamesLost = 1, Ranking = 2},
            };
            var groupDStanding = new List<PlayerStanding>
            {
                new PlayerStanding { Player = players[6], GamesWon = 2, GamesLost = 2, Ranking = 1},
                new PlayerStanding { Player = players[7], GamesWon = 1, GamesLost = 3, Ranking = 2}
            };

            var groupOfPlayerStanding = new Dictionary<string, List<PlayerStanding>>
            {
                {"Group A", groupAStanding},
                {"Group B", groupBStanding},
                {"Group C", groupCStanding},
                {"Group D", groupDStanding},

            };

            _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.Players)).Returns(Task.CompletedTask);

            // Act
            var matches = await _createKnockOutMatches.CreateFirstRoundMatchesAfterGroup(tournament, matchType, groupOfPlayerStanding);

            // Assert
            Assert.NotNull(matches);
            Assert.Equal(4, matches.Count());
        }

        [Fact]
        public async Task CreateFirstRoundMatchesAfterGroup_WithOddGroupOfPlayerStanding_ReturnsMatches()
        {
            // Arrange
            var tournament = TournamentFixtures.GetKnockoutTournament();
            var players = PlayerFixtures.GetSamplePlayers(8).ToList();
            var matchType = new KnockOut { Name = "Knockout Test", Players = players};

            var groupAStanding = new List<PlayerStanding> 
            {
                new PlayerStanding { Player = players[0], GamesWon = 4, GamesLost = 1, Ranking = 1},
                new PlayerStanding { Player = players[1], GamesWon = 3, GamesLost = 1, Ranking = 2},
            };
            var groupBStanding = new List<PlayerStanding> 
            {

                new PlayerStanding { Player = players[2], GamesWon = 3, GamesLost = 2, Ranking = 1},
                new PlayerStanding { Player = players[3], GamesWon = 2, GamesLost = 3, Ranking = 2},
            };
            var groupCStanding = new List<PlayerStanding>
            {

                new PlayerStanding { Player = players[4], GamesWon = 4, GamesLost = 1, Ranking = 1},
                new PlayerStanding { Player = players[5], GamesWon = 3, GamesLost = 1, Ranking = 2},
            };

            var groupOfPlayerStanding = new Dictionary<string, List<PlayerStanding>>
            {
                {"Group A", groupAStanding},
                {"Group B", groupBStanding},
                {"Group C", groupCStanding},
            };

            _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.Players)).Returns(Task.CompletedTask);

            // Act
            var matches = await _createKnockOutMatches.CreateFirstRoundMatchesAfterGroup(tournament, matchType, groupOfPlayerStanding);

            // Assert
            Assert.NotNull(matches);
            Assert.Equal(4, matches.Count());
            Assert.Equal(4, groupOfPlayerStanding.Count); // A bye group was added making it 3+1=4

            var byeMatchesCount = matches.Count(m => m.FirstPlayer.Name == Utility.ByePlayerName || m.SecondPlayer.Name == Utility.ByePlayerName);
            Assert.Equal(2, byeMatchesCount);

            //assert the bye matches are from the same group of player
            var byeMatchGroups = matches.Where(m => m.FirstPlayer.Name == Utility.ByePlayerName || m.SecondPlayer.Name == Utility.ByePlayerName)
                .Select(m => groupOfPlayerStanding.FirstOrDefault(g => g.Value.Any(ps => ps.Player == m.FirstPlayer || ps.Player == m.SecondPlayer))).ToList();
            var firstByeMatchGroup = byeMatchGroups.First();
            var secondByeMatchGroup = byeMatchGroups.Skip(1).First();
            Assert.Equal(firstByeMatchGroup, secondByeMatchGroup);
        }

    //     [Fact]
    //     public async Task CreateSubsequentMatches_WithValidTournamentAndMatchType_ReturnsMatches()
    //     {
    //         // Arrange
    //         var tournament = TournamentFixtures.GetKnockoutTournament();
    //         var matchType = new MatchType { Players = PlayerFixtures.GetSamplePlayers(8).ToList() };
    //         var previousRound = new Round { Matches = new List<Match> { new Match { Winner = PlayerFixtures.GetSamplePlayers(1).First() } } };
    //         _matchTypeRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(matchType, mt => mt.Players)).Returns(Task.CompletedTask);
    //         _roundRepositoryMock.Setup(r => r.ExplicitLoadCollectionAsync(previousRound, r => r.Matches)).Returns(Task.CompletedTask);

    //         // Act
    //         var matches = await _createKnockOutMatches.CreateSubsequentMatches(tournament, matchType);

    //         // Assert
    //         Assert.NotNull(matches);
    //         Assert.True(matches.Count() > 0);
    //     }
    // }
}

