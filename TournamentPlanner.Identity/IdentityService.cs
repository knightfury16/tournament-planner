using TournamentPlanner.Application.Common.Interfaces;

namespace TournamentPlanner.Identity;

public class IdentityService : IIdentityService
{
    public Task<bool> AddRoleToApplicationUserAsync(string userId, string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LoginApplicationUserAsync(string email, string password, bool persistent = false)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterApplicationUserAndSigninAsync(string username, string email, string password, bool persistent = false)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterApplicationUserAsync(string username, string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SignoutApplicationUserAsync()
    {
        throw new NotImplementedException();
    }
}
