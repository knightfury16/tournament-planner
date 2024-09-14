using TournamentPlanner.Application.DTOs;

namespace TournamentPlanner.Application;

public class MatchTypeDto
{
    public int Id { get; set; }
    public required string Name { get; set; } //group name, eg. GroupA or Elimination1

    //TODO: Make a RoundDto and map it to it
    //public int Round { get; set; }
    //I dont need the players and matches unless specificlly required
    public List<PlayerDto> Players { get; set; } = new();
    // public List<MatchDto> Matches { get; set; } = new();
}
