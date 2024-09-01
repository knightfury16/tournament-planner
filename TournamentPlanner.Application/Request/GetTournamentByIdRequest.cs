using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request;
public class GetTournamentByIdRequest : IRequest<TournamentDto>
{

    public readonly int Id;
    public GetTournamentByIdRequest(int id)
    {
        Id = id;
    }

}