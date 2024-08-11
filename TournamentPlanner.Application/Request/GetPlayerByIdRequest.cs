using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request
{
    public class GetPlayerByIdRequest: IRequest<Player>
    {
        public readonly int id;

        public GetPlayerByIdRequest(int id)
        {
            this.id = id;
        }
        
    }
}