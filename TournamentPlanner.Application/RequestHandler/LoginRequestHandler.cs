using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class LoginRequestHandler : IRequestHandler<LoginRequest, UserInfoDto>
{
  private readonly IIdentityService _identityService;
  private readonly ICurrentUser _currentUser;

  public LoginRequestHandler(IIdentityService identityService, ICurrentUser currentUser)
  {
    _identityService = identityService;
    _currentUser = currentUser;
  }

  public async Task<UserInfoDto?> Handle(LoginRequest request, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

    var applicationDto = new ApplicationUserDto
    {
      Email = request.LoginRequestDto.Email,
      Password = request.LoginRequestDto.Password
    };

    var result = await _identityService.LoginApplicationUserAsync(applicationDto, request.IsPersistent);

    if (!result) throw new BadRequestException("Loign failed. Please provide valid email and password");

    var userInfo = new UserInfoDto
    {
      Email = _currentUser.Email!,
      Name = _currentUser.Name!,
      Role = _currentUser.Role!,
    };

    return userInfo;
  }
}
