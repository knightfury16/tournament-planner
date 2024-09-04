using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchTypesRequest : IRequest<IEnumerable<MatchTypeDto>>
{
    public int Id { get; set; }
    public GetTournamentMatchTypesRequest(int Id)
    {
        this.Id = Id;
    }

}
