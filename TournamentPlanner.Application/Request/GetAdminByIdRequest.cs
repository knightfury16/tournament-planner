using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminByIdRequest : IRequest<AdminDto>
{

    public readonly int id;

    public GetAdminByIdRequest(int id)
    {
        this.id = id;
    }
}
