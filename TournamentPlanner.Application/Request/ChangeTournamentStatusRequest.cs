using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class ChangeTournamentStatusRequest : IRequest<bool>
{
    public TournamentStatus TournamentStatus { get; set; }
    public int TournamentId { get; set; }

    public ChangeTournamentStatusRequest(TournamentStatus tournamentStatus, int tournamentId)
    {
        TournamentStatus = tournamentStatus;
        TournamentId = tournamentId;
    }

}
