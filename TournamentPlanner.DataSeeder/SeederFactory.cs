using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TournamentPlanner.DataModeling;

namespace TournamentPlanner.DataSeeder
{
    public class SeederFactory : IDesignTimeDbContextFactory<TournamentPlannerDataContext>
    {
        public bool ON_TEST { get; set; }
        public SeederFactory(bool? onTest = true)
        {
            ON_TEST = onTest ?? false;
        }
        public SeederFactory()
        {
            ON_TEST = true;
        }
        public TournamentPlannerDataContext CreateDbContext(string[]? args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var myConnection = configuration.GetConnectionString("DefaultConnection");
            if (!ON_TEST)
            {
                myConnection = configuration.GetConnectionString("TestConnection");
            }

            var optionsBuilder = new DbContextOptionsBuilder<TournamentPlannerDataContext>();
            Console.WriteLine("Connection string: " + myConnection);
            optionsBuilder.UseNpgsql(myConnection);

            return new TournamentPlannerDataContext(optionsBuilder.Options);
        }


    }
}