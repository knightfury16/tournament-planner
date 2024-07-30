using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TournamentPlanner.Infrastructure.DataContext
{
    public class TournamentPlannerContextFactory : IDesignTimeDbContextFactory<TournamentPlannerDataContext>
    {
        public TournamentPlannerDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<TournamentPlannerDataContext>();
            optionsBuilder.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]);

            return new TournamentPlannerDataContext(optionsBuilder.Options);
        }
    }
}