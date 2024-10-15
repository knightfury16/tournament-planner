using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class SimulateMatchesRequest : IRequest<bool>
{
    public int TournamentId { get; }
    public SimulateMatchesRequest(int TournamentId)
    {
        this.TournamentId = TournamentId;
    }

}
