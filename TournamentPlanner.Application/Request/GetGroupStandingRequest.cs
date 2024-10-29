using TournamentPlanner.Domain;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetGroupStandingRequest: IRequest<IEnumerable<PlayerStanding>>
{
    public int GroupId { get; set; }

    public GetGroupStandingRequest(int groupId)
    {
        GroupId = groupId;
    }


}
