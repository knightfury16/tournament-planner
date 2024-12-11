using System.Security.Claims;
using TournamentPlanner.Application.DTOs;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> RegisterApplicationUserAsync(ApplicationUserDto applicationUserDto);
    Task<bool> RegisterApplicationUserAndSigninAsync(ApplicationUserDto applicationUserDto, bool persistent = false);
    Task<bool> LoginApplicationUserAsync(ApplicationUserDto applicationUserDto, bool persistent = false);
    Task SignoutApplicationUserAsync();
    Task AddRoleToApplicationUserAsync(string email, string roleName);
    Task<bool> CreateRoleAsync(string roleName);
    Task AddClaimToApplicationUserAsync(string email, string claimType, string claimValue);
    Task<bool> CheckUserClaimAsync(string email, string claimType, string claimValue);
    Task<List<Claim>> GetAllClaimsOfApplicationUser(string email);
    Task<List<Claim>> GetAllClaimsOfApplicationUser(ClaimsPrincipal claimsPrincipal);
    Task<List<Claim>> GetAllClaimsOfCurrentApplicationUser();
    Task<List<string>> GetAllRolesOfUser(string email);
    Task<List<Claim>> GetRoleClaimsOfuser(string email);
}
