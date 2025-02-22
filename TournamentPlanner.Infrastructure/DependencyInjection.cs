using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.DataModeling;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<TournamentPlannerDataContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                );
            });
            services.AddScoped<IRepository<Player>, Repository<Player>>();
            services.AddScoped<IRepository<Match>, Repository<Match>>();
            services.AddScoped<IRepository<Tournament>, Repository<Tournament>>();
            services.AddScoped<IRepository<Round>, Repository<Round>>();
            services.AddScoped<IRepository<Admin>, Repository<Admin>>();
            services.AddScoped<IRepository<MatchType>, Repository<MatchType>>();
            services.AddScoped<IRepository<Draw>, Repository<Draw>>();
        }
    }
}

