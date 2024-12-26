using TournamentPlanner.Application.DTOs;

namespace TournamentPlanner.Application;
public class DrawDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required TournamentDto Tournament { get; set; }
    public required MatchTypeDto MatchType { get; set; }
}
