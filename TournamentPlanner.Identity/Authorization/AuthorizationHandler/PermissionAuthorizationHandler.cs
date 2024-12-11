using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Identity.Authorization.AuthorizationRequirement;

namespace TournamentPlanner.Identity.Authorization.AuthorizationHandler;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;

    public PermissionAuthorizationHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var contextUser = context.User;
        var email = contextUser.FindFirstValue(ClaimTypes.Email);
        if (email == null) return;

        var permission = await _permissionService.HasPermissionAsync(email, requirement.Permission);

        if (permission)
        {
            context.Succeed(requirement);
        }
    }
}
