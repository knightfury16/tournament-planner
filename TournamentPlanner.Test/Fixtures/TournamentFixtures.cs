using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Test.Fixtures
{
    public static class TournamentFixtures
    {
        private static readonly Admin TestAdmin = new()
        {
            Id = 1,
            Name = "Test Admin",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
        };

        public static Tournament GetTournament(int id = 1)
        {
            return new Tournament
            {
                Id = id,
                Name = $"Test Tournament {id}",
                CreatedBy = TestAdmin,
                Status = TournamentStatus.RegistrationClosed,
                GameType = new GameType { Name = GameTypeSupported.Chess },
                StartDate = DateTime.UtcNow.AddDays(2),
                EndDate = DateTime.UtcNow.AddDays(3),
                TournamentType = TournamentType.GroupStage, //default
                MaxParticipant = 64,
            };
        }

        public static Tournament GetGroupTournament()
        {
            return GetTournament();
        }

        public static Tournament GetKnockoutTournament()
        {
            var tournament = GetTournament();
            tournament.TournamentType = TournamentType.Knockout;
            return tournament;
        }

        //Tournaments Type are mixed
        public static List<Tournament> GetTournaments()
        {
            return new List<Tournament>
            {
                new Tournament
                {
                    Id = 1,
                    Name = "Test Tournament 1",
                    CreatedBy = TestAdmin,
                    Status = TournamentStatus.Ongoing,
                    GameType = new GameType { Name = GameTypeSupported.Chess },
                    StartDate = DateTime.UtcNow.AddDays(2),
                    EndDate = DateTime.UtcNow.AddDays(3),
                    TournamentType = TournamentType.GroupStage,
                },
                new Tournament
                {
                    Id = 2,
                    Name = "Another Tournament",
                    CreatedBy = TestAdmin,
                    Status = TournamentStatus.Draft,
                    GameType = new GameType { Name = GameTypeSupported.TableTennis },
                    StartDate = DateTime.UtcNow.AddDays(-1),
                    EndDate = DateTime.UtcNow.AddDays(2),
                    TournamentType = TournamentType.Knockout,
                },
                new Tournament
                {
                    Id = 3,
                    Name = "Test Tournament 2",
                    CreatedBy = TestAdmin,
                    Status = TournamentStatus.Completed,
                    GameType = new GameType { Name = GameTypeSupported.Chess },
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    EndDate = DateTime.UtcNow.AddDays(-2),
                    TournamentType = TournamentType.GroupStage,
                },
                new Tournament
                {
                    Id = 4,
                    Name = "Upcoming Test",
                    CreatedBy = TestAdmin,
                    Status = TournamentStatus.Ongoing,
                    GameType = new GameType { Name = GameTypeSupported.TableTennis },
                    StartDate = DateTime.UtcNow.AddDays(5),
                    EndDate = DateTime.UtcNow.AddDays(10),
                    TournamentType = TournamentType.GroupStage,
                },
                new Tournament
                {
                    Id = 5,
                    Name = "Upcoming knockout Test",
                    CreatedBy = TestAdmin,
                    Status = TournamentStatus.Ongoing,
                    GameType = new GameType { Name = GameTypeSupported.TableTennis },
                    StartDate = DateTime.UtcNow.AddDays(5),
                    EndDate = DateTime.UtcNow.AddDays(10),
                    TournamentType = TournamentType.Knockout,
                },
            };
        }

        public static Tournament GetPopulatedGroupTournamentFresh(int playerCount = 16)
        {
            var tournament = GetTournament();
            var players = PlayerFixtures.GetSamplePlayers(playerCount);
            tournament.Participants.AddRange(players);

            return tournament;
        }
    }
}
