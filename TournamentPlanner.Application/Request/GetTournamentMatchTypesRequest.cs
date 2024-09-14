using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchTypesRequest : IRequest<IEnumerable<MatchTypeDto>>
{
    public int tournamentId { get; set; }
    public GetTournamentMatchTypesRequest(int Id)
    {
        this.tournamentId = Id;
    }

}
