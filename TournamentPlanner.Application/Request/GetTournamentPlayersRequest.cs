using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentPlayersRequest : IRequest<IEnumerable<PlayerDto>>
{
    public int TournamentId { get; set; }
    public GetTournamentPlayersRequest(int Id)
    {
        TournamentId = Id;
    }

}
