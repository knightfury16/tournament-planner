using Microsoft.AspNetCore.Authorization;
using TournamentPlanner.Application.Common.AuthorizationRequirement;

namespace TournamentPlanner.Identity.Authorization.AuthorizationRequirement;

public class PermissionRequirement : IPermissionApplicationRequirement, IAuthorizationRequirement
{
    public string Permission { get; set; }
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
