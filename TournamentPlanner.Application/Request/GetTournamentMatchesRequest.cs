using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;
//Getting the matches through populated Draws
public class GetTournamentMatchesRequest : IRequest<IEnumerable<DrawDto>>
{
    public int Id { get; set; }
    public GetTournamentMatchesRequest(int Id)
    {
        this.Id = Id;
    }
}
