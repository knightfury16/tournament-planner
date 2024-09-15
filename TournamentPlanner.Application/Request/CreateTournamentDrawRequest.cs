using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CreateTournamentDrawRequest : IRequest<IEnumerable<DrawDto>>
{
    public int TournamentId { get; set; }
    public List<int>? SeedersId { get; set; }// Seeders players Id
    public string? MatchTypePrefix { get; set; } = null;
    public CreateTournamentDrawRequest(int tournamentId, List<int>? seeders = null, string? matchTypePrefix = null)
    {
        TournamentId = tournamentId;
        SeedersId = seeders;
        MatchTypePrefix = matchTypePrefix;
    }

}