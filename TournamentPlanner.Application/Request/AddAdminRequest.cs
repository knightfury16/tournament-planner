using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class AddAdminRequest : IRequest<AdminDto>
{
    public AddAdminDto AddAdminDto { get; set; }

    public AddAdminRequest(AddAdminDto addAdminDto)
    {
        AddAdminDto = addAdminDto;
    }

}
