using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Infrastructure.DataContext;

namespace TournamentPlanner.Infrastructure
{
    public static class DependencyInjection
    {
       public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration){
            services.AddDbContext<TournamentPlannerDataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
       } 
    }
}