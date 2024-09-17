using TournamentPlanner.Application.DTOs;

namespace TournamentPlanner.Application;
public class DrawDto
{
    public required TournamentDto Tournament { get; set; }
    public required MatchTypeDto MatchType { get; set; }
}
