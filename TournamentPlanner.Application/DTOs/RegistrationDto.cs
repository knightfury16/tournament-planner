namespace TournamentPlanner.Application.DTOs;
public class RegistrationInTournamentDto
{
    public int TournamentId { get; set; }
    //need some mechanism to validate a valid player
    public required int PlayerId { get; set; }
}