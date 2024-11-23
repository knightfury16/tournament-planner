using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TournamentPlanner.Identity;

public static class DependencyInjection
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TpIdentityDbContex>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
