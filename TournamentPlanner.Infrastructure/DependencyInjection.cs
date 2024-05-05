using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Infrastructure.DataContext;

namespace TournamentPlanner.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TournamentPlannerDataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepository<Player, Player>, Repository<Player, Player>>();
            services.AddScoped<IRepository<Match, Match>, Repository<Match, Match>>();
            services.AddScoped<IRepository<Tournament, Tournament>, Repository<Tournament,Tournament>>();
            services.AddScoped<IRepository<Round, Round>, Repository<Round, Round>>();
        }
    }
}