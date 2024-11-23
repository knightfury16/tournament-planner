namespace TournamentPlanner.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> RegisterApplicationUserAsync(string username, string email, string password);
    Task<bool> RegisterApplicationUserAndSigninAsync(string username, string email, string password, bool persistent = false);
    Task<bool> LoginApplicationUserAsync(string email, string password, bool persistent = false);
    Task<bool> SignoutApplicationUserAsync();
    Task<bool> AddRoleToApplicationUserAsync(string userId, string roleName);
    Task<bool> CreateRoleAsync(string roleName);
}
