using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TournamentPlanner.DataModeling
{
    public class TournamentPlannerDataContextFactory : IDesignTimeDbContextFactory<TournamentPlannerDataContext>
    {
        public TournamentPlannerDataContext CreateDbContext(string[]? args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var myConnection = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TournamentPlannerDataContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new TournamentPlannerDataContext(optionsBuilder.Options);
        }
    }
}