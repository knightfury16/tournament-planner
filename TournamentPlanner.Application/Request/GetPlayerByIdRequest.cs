using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request
{
    public class GetPlayerByIdRequest: IRequest<FullPlayerDto>
    {
        public readonly int id;

        public GetPlayerByIdRequest(int id)
        {
            this.id = id;
        }
        
    }
}