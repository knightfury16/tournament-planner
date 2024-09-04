using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentPlayersRequest : IRequest<IEnumerable<PlayerDto>>
{
    public int Id { get; set; }
    public GetTournamentPlayersRequest(int Id)
    {
        this.Id = Id;
    }

}
