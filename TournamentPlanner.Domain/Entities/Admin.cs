namespace TournamentPlanner.Domain.Entities;

public class Admin : User
{
    public required string PhoneNumber { get; set; }
    public List<Tournament>? CreatedTournament { get; set; }
}
