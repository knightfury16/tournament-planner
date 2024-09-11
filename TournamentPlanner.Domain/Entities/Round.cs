using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{

    //I need round. Say in Group A Round 1 Matchers are {1vs2,3vs4,5bye}
    //I need round. Say in Group A Round 2 Matchers are {1vs5,3vs2,4bye} ...so on and so forth
    //For knockout RoundName maybe "Quarter Final" while MatchType name be Finale
    public class Round : BaseEntity
    {
        public string RoundName { get; set; } = string.Empty;
        public int RoundNumber { get; set; }
        public DateTime? StartTime { get; set; }

        public List<Match> Matches { get; set; } = new();
        public required MatchType MatchType { get; set; }
        public int MatchTypeId { get; set; }
        public bool IsCompleted { get; set; }

    }
}