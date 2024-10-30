using TournamentPlanner.Domain;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetGroupStandingRequest: IRequest<IEnumerable<PlayerStandingDto>>
{
    public int GroupId { get; set; }

    public GetGroupStandingRequest(int groupId)
    {
        GroupId = groupId;
    }


}
