using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Constant;
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

        // Email as primary key
        modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();

        SeedDefaultRoleWithClaim(modelBuilder);
    }

    private void SeedDefaultRoleWithClaim(ModelBuilder modelBuilder)
    {
        var defaultRoles = new List<IdentityRole>
        {
            new IdentityRole { Name = Role.Admin.ToString(), NormalizedName = Role.Admin.ToString().ToUpper() },
            new IdentityRole { Name = Role.Moderator.ToString(), NormalizedName = Role.Moderator.ToString().ToUpper() },
            new IdentityRole { Name = Role.Player.ToString(), NormalizedName = Role.Player.ToString().ToUpper() }
        };

        // Check if roles already exist
        foreach (var role in defaultRoles)
        {
            var existingRole = modelBuilder.Entity<IdentityRole>().Metadata.FindPrimaryKey()?.Properties
                .FirstOrDefault(r => r.Name == role.Name);
            if (existingRole == null)
            {
                Console.WriteLine($"Creating role: {role.Name}");
                modelBuilder.Entity<IdentityRole>().HasData(role);
            }
        }


        var identityRoleClaim = new List<IdentityRoleClaim<string>>();
        // Log the created roles for debugging
        foreach (var role in defaultRoles)
        {
            Console.WriteLine($"Creating role: {role.Name} with ID: {role.Id}");
        }

        foreach (var role in defaultRoles)
        {
            if (role.Name == Role.Admin.ToString()) AddAdminPermission(identityRoleClaim, role);
            if (role.Name == Role.Moderator.ToString()) AddModeratorPermission(identityRoleClaim, role);
            if (role.Name == Role.Player.ToString()) AddPlayerPermission(identityRoleClaim, role);
        }
        foreach (var claim in identityRoleClaim)
        {
            Console.WriteLine($"Role ID: {claim.RoleId}, Claim Type: {claim.ClaimType}, Claim Value: {claim.ClaimValue}");
        }

        modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(identityRoleClaim);
    }

    private void AddModeratorPermission(List<IdentityRoleClaim<string>> identityRoleClaim, IdentityRole role)
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 1, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Read }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 2, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Edit }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 3, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.AddScore }
        );
    }

    private void AddAdminPermission(List<IdentityRoleClaim<string>> identityRoleClaim, IdentityRole role)
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 4, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Read }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 5, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Edit }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 6, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Create }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 7, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Delete }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 8, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.AddScore }
        );
    }
    private void AddPlayerPermission(List<IdentityRoleClaim<string>> identityRoleClaim, IdentityRole role)
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string> { Id = 9, RoleId = role.Id, ClaimType = DomainClaim.PermissionClaimType, ClaimValue = Policy.Read }
        );
    }
}
