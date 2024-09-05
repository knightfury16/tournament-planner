using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class RegisterPlayerInTournamentRequest : IRequest<bool>
{
    public RegistrationInTournamentDto RegistrationInTournamentDto { get; }
    public RegisterPlayerInTournamentRequest(RegistrationInTournamentDto registrationInTournamentDto)
    {
        RegistrationInTournamentDto = registrationInTournamentDto;
    }

}
