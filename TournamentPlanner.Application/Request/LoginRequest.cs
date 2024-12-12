using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class LoginRequest : IRequest<UserInfoDto>
{
  public LoginDto LoginRequestDto { get; set; }
  public bool IsPersistent = true;

  public LoginRequest(LoginDto loginRequestDto)
  {
    LoginRequestDto = loginRequestDto;
  }
}
