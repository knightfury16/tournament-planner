using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class LogoutRequestHandler : IRequestHandler<LogoutRequest, bool>
{
  private readonly IIdentityService _identityService;

  public LogoutRequestHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken = default)
  {
    await _identityService.SignoutApplicationUserAsync();
    return true;
  }
}
