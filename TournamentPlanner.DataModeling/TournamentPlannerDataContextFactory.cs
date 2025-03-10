using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TournamentPlanner.DataModeling
{
    public class TournamentPlannerDataContextFactory
        : IDesignTimeDbContextFactory<TournamentPlannerDataContext>
    {
        public TournamentPlannerDataContext CreateDbContext(string[]? args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
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

            var optionsBuilder = new DbContextOptionsBuilder<TournamentPlannerDataContext>();
            Console.WriteLine("Connection string: " + connectionString);
            optionsBuilder.UseNpgsql(connectionString);

            return new TournamentPlannerDataContext(optionsBuilder.Options);
        }
    }
}

