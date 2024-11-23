using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TournamentPlanner.Identity;

public class TpIdentityDbContextFactory : IDesignTimeDbContextFactory<TpIdentityDbContex>
{
    public TpIdentityDbContex CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENV");
        string connectionString;
        if (environment == "test")
        {
            connectionString = configuration.GetConnectionString("TestConnection")!;
        }
        else
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        var optionsBuilder = new DbContextOptionsBuilder<TpIdentityDbContex>();
        optionsBuilder.UseNpgsql(connectionString);

        return new TpIdentityDbContex(optionsBuilder.Options);
    }
}
