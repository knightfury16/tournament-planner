using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request;
public class AddTournamentRequest : IRequest<TournamentDto>
{
    public AddTournamentDto AddTournamentDto { get; set; }
    public AddTournamentRequest(AddTournamentDto addTournamentDto)
    {
        AddTournamentDto = addTournamentDto;
    }

}