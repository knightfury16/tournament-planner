using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.DataSeeder
{
    public class TournamentBuilder
    {
        private Tournament _tournament = new Tournament()
        {
            Name = "",
            CreatedBy = new AdminBuilder().Build(),
            GameType = new GameType() { Name = GameTypeSupported.TableTennis },
            TournamentType = TournamentType.GroupStage,
            StartDate = DateTime.UtcNow.AddDays(12),
            EndDate = DateTime.UtcNow.AddDays(20),
        };

        public TournamentBuilder WithName(string name)
        {
            _tournament.Name = name;
            return this;
        }

        public TournamentBuilder WithAdmin(Admin admin)
        {
            _tournament.CreatedBy = admin;
            return this;
        }

        public TournamentBuilder WithGameType(GameType gameType)
        {
            _tournament.GameType = gameType;
            return this;
        }

        public TournamentBuilder WithStartDate(DateTime startDate)
        {
            _tournament.StartDate = startDate;
            _tournament.EndDate = startDate.AddDays(7);
            return this;
        }
        public TournamentBuilder WithStatus(TournamentStatus status)
        {
            _tournament.Status = status;
            return this;
        }

        public TournamentBuilder WithTournamentType(TournamentType tournamentType)
        {
            _tournament.TournamentType = tournamentType;
            return this;
        }

        public Tournament Build()
        {
            _tournament.CreatedAt = DateTime.UtcNow;
            _tournament.UpdatedAt = DateTime.UtcNow;
            _tournament.Status = TournamentStatus.Draft;
            return _tournament;
        }
    }
}