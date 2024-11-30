using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class LoginRequest : IRequest<bool>
{
  public LoginDto LoginRequestDto { get; set; }
  public bool IsPersistent = true;

  public LoginRequest(LoginDto loginRequestDto)
  {
    LoginRequestDto = loginRequestDto;
  }
}
