using TournamentPlanner.Domain;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetGroupStandingRequestHandler : IRequestHandler<GetGroupStandingRequest, IEnumerable<PlayerStanding>>
{
    public Task<IEnumerable<PlayerStanding>?> Handle(GetGroupStandingRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
