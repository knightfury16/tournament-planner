using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class CreateMatchTypeRequest : IRequest<IEnumerable<MatchType>>
{
    public int TournamentId { get; set; }
    public CreateMatchTypeRequest(int id)
    {
        TournamentId = id;
    }

}
