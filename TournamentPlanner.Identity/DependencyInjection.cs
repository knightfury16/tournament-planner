using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Identity.Authorization.AuthorizationHandler;
using TournamentPlanner.Identity.Services;

namespace TournamentPlanner.Identity;

public static class DependencyInjection
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TpIdentityDbContex>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<TpIdentityDbContex>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
