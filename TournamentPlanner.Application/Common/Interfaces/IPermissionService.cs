namespace TournamentPlanner.Application.Common.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string email, string policy);
    Task<IEnumerable<string>> GetUserPermissionAsync(string email);
}
