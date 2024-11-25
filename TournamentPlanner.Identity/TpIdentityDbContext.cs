using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Identity;

public class TpIdentityDbContex : IdentityDbContext<ApplicationUser>
{
    public TpIdentityDbContex(DbContextOptions options) : base(options)
    {

    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //seeding default domain role in db
        var defaultRoles = Enum.GetValues(typeof(DomainRole)).Cast<DomainRole>()
                           .Select((role, index) => new IdentityRole { Name = role.ToString(), NormalizedName = role.ToString().ToUpper() });
        modelBuilder.Entity<IdentityRole>().HasData(defaultRoles);
    }


}
