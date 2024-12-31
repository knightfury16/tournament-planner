using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CanIMakeDrawRequest : IRequest<CanIDrawDto>
{
    public int TournamentId { get; set; }
    public CanIMakeDrawRequest(int tournamentId)
    {
        TournamentId = tournamentId;
    }

}
