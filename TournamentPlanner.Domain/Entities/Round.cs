using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{

    //@depricated
    public class Round: BaseEntity
    {
        public int RoundNumber { get; set; }

        public DateTime? StartTime { get; set; }

    }
}