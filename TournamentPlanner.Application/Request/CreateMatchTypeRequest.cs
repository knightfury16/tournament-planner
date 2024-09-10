using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class CreateMatchTypeRequest : IRequest<IEnumerable<MatchTypeDto>>
{
    public int TournamentId { get; set; }
    public CreateMatchTypeRequest(int id)
    {
        TournamentId = id;
    }

}
