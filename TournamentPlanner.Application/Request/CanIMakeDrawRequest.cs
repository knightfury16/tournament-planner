using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CanIMakeDrawRequest : IRequest<bool>
{
    public int TournamentId { get; set; }
    public CanIMakeDrawRequest(int tournamentId)
    {
        TournamentId = tournamentId;
    }

}
