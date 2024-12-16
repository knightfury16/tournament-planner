using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetUserInfoRequestHandler : IRequestHandler<GetUserInfoRequest, UserInfoDto>
{
  private readonly ICurrentUser _currentUser;
  public GetUserInfoRequestHandler(ICurrentUser currentUser)
  {
    _currentUser = currentUser;
  }
  public Task<UserInfoDto?> Handle(GetUserInfoRequest request, CancellationToken cancellationToken = default)
  {
    UserInfoDto? userInfo;

    if (_currentUser.IsAuthenticated)
    {
      userInfo = new UserInfoDto
      {
        Email = _currentUser.Email!,
        Name = _currentUser.Name!,
        Role = _currentUser.Role!,
      };
    }
    else
    {
      userInfo = null;
    }

    return Task.FromResult(userInfo);
  }
}
