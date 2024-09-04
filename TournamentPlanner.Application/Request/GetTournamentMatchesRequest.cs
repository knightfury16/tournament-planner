using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchesRequest : IRequest<IEnumerable<MatchDto>>
{
    public int Id { get; set; }
    public GetTournamentMatchesRequest(int Id)
    {
        this.Id = Id;
    }
}
