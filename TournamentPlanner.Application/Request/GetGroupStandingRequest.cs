using TournamentPlanner.Domain;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetGroupStandingRequest: IRequest<IEnumerable<PlayerStanding>>
{
    public int TournamentId { get; set; }
    public int GroupId { get; set; }

    public GetGroupStandingRequest(int tournamentId, int groupId)
    {
        TournamentId = tournamentId;
        GroupId = groupId;
    }


}
