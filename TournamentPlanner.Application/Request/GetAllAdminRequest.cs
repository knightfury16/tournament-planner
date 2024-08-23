using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAllAdminRequest : IRequest<IEnumerable<AdminDto>>
{
    private string? _name;
    public string? Name { 
        get => _name;
        set => _name = value?.ToLower();
     }
    public GetAllAdminRequest(string? name)
    {
        Name = name;
    }
    public GetAllAdminRequest()
    {
    }
}
