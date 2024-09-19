namespace TournamentPlanner.Application.DTOs;

public class RoundDto
{
        public string RoundName { get; set; } = string.Empty;
        public int RoundNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public List<MatchDto> Matches { get; set; } = new();
        public bool IsCompleted { get; set; }
}
