using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TournamentPlanner.Identity;

public class TpIdentityDbContex : IdentityDbContext<ApplicationUser>
{
    public TpIdentityDbContex(DbContextOptions options) : base(options)
    {

    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
