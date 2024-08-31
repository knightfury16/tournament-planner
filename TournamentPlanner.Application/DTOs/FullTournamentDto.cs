namespace TournamentPlanner.Application.DTOs
{
    public class FullTournamentDto : TournamentDto
    {
        public required AdminDto CreatedBy { get; set; }
        public List<PlayerDto> Participants { get; set; } = new();
        public List<MatchDto> Matches { get; set; } = new();

    }
}