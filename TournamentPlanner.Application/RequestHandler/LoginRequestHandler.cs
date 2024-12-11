using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class LoginRequestHandler : IRequestHandler<LoginRequest, bool>
{
  private readonly IIdentityService _identityService;

  public LoginRequestHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task<bool> Handle(LoginRequest request, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

    var applicationDto = new ApplicationUserDto
    {
      Email = request.LoginRequestDto.Email,
      Password = request.LoginRequestDto.Password
    };

    var result = await _identityService.LoginApplicationUserAsync(applicationDto, request.IsPersistent);

    if (!result) throw new BadRequestException("Loign failed. Please provide valid email and password");

    return result;
  }
}
