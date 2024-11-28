using System.Security.Claims;
using TournamentPlanner.Application.Common.Interfaces;

namespace TournamentPlanner.Api.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpcontextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpcontextAccessor = httpContextAccessor;
    }

    public string? Name => _httpcontextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    public string? Email => _httpcontextAccessor.HttpContext?.User?.FindFirst("Email")?.Value;
    public int? DomainUserId => _httpcontextAccessor.HttpContext?.User?.FindFirst("DomainUserId")?.Value != null ? int.Parse(_httpcontextAccessor.HttpContext.User.FindFirst("DomainUserId")!.Value) : null;
    public string? ApplicationUserId => _httpcontextAccessor.HttpContext?.User?.FindFirst("ApplicationUserId")?.Value;
}
