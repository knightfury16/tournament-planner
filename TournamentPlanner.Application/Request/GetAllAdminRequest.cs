using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAllAdminRequest : IRequest<IEnumerable<AdminDto>>
{
    public string? Name { get; set; }
    public GetAllAdminRequest(string? name)
    {
        Name = name;
    }
    public GetAllAdminRequest()
    {
    }
}
