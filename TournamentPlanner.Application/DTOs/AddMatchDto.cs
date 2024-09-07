using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs;

public class AddMatchDto
{

    [Required]
    public int Player1Id { get; set; }
    [Required]
    public int Player2Id { get; set; }
    public int MatchTypeId { get; set; }
    [Required]
    public required object GameSpecificScore { get; set; }

}
