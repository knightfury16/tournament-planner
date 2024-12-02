using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.DataModeling;
using TournamentPlanner.Identity.Authorization.AuthorizationHandler;
using TournamentPlanner.Identity.Model;
using TournamentPlanner.Identity.Services;

namespace TournamentPlanner.Identity;

public static class DependencyInjection
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<TournamentPlannerDataContext>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
