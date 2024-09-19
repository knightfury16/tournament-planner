using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request;

public class GetTournamentDrawRequest: IRequest<IEnumerable<DrawDto>>
{
    public int TournamentId { get; set; }
    public GetTournamentDrawRequest(int id)
    {
        TournamentId = id;
    }
    
}
