namespace TournamentPlanner.Application;

public class AddMatchScoreDto
{
    public DateTime? GamePlayed { get; set; }
    public required object GameSpecificScore { get; set; }
    //TODO: can i pass the tournament game type here to double validate the in handler? or will it be redundent?

}
