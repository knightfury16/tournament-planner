using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Constant;

namespace TournamentPlanner.Identity.Services;

public class PermissionService : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<string>> GetUserPermissionAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Enumerable.Empty<string>();

        var userRoles = await GetUserRolesAsync(user);
        var rolesPermissionClaims = await GetRolesPermissionClaims(userRoles);

        return rolesPermissionClaims.Select(rp => rp.Value);
    }

    private async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<bool> HasPermissionAsync(string email, string policy)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var userRoles = await GetUserRolesAsync(user);
        var rolesPermissionClaims = await GetRolesPermissionClaims(userRoles);

        if (rolesPermissionClaims.Any(rc => rc.Value == policy)) return true;

        return false;
    }

    private async Task<List<Claim>> GetRolesPermissionClaims(List<string> userRoles)
    {
        var rolesPermissionClaims = new List<Claim>();
        foreach (var role in userRoles)
        {
            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole == null) continue;
            var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
            rolesPermissionClaims.AddRange(roleClaims.Where(rc => rc.Type == DomainClaim.PermissionClaimType));
        }
        return rolesPermissionClaims;
    }
}
