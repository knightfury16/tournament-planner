namespace TournamentPlanner.DataModeling;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Constant;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Identity.Model;

public class TournamentPlannerDataContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<GameType> GameTypes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchType> MatchTypes { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<KnockOut> KnockOuts { get; set; }

    //TODO: Configure its relations manully to be exact
    public DbSet<Draw> Draws { get; set; }
    public DbSet<SeededPlayer> SeededPlayers { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public TournamentPlannerDataContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //for identity integration
        base.OnModelCreating(modelBuilder);

        //* Application User configuration
        // Email as primary key
        modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();

        //* Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(p => p.Age).IsRequired();
            entity.Property(p => p.Weight).IsRequired();
            entity.HasIndex(a => a.Email).IsUnique();
        });

        //* Admin configuration
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(a => a.PhoneNumber).IsRequired();
            entity.HasIndex(a => a.Email).IsUnique();
        });

        //* Match configuration
        modelBuilder.Entity<Match>(entity =>
        {
            // fields
            entity.Property(m => m.ScoreJson).HasColumnType("jsonb");

            entity.HasOne(m => m.FirstPlayer).WithMany().HasForeignKey(m => m.Player1Id);

            entity.HasOne(m => m.SecondPlayer).WithMany().HasForeignKey(m => m.Player2Id);

            entity.HasOne(m => m.Winner).WithMany().HasForeignKey(m => m.WinnerId);

            entity
                .HasOne(m => m.RescheduledBy)
                .WithOne()
                .HasForeignKey<Match>(m => m.RescheduledById);

            entity
                .HasOne(m => m.Round)
                .WithMany(r => r.Matches)
                .HasForeignKey(m => m.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        //tournament configuration
        modelBuilder.Entity<Tournament>(entity =>
        {
            //filed config
            entity.Property(p => p.Name).HasMaxLength(250).IsRequired();

            entity.Property(p => p.StartDate).IsRequired();

            entity.Property(p => p.MaxParticipant).HasDefaultValue<int>(104); // 26 Group (A-Z) * 4 per group = 104

            entity.Property(p => p.RegistrationFee).HasColumnType("decimal(18,2)");

            entity.Property(p => p.MinimumAgeOfRegistration).HasDefaultValue<int>(18);

            entity.Property(p => p.WinnerPerGroup).HasDefaultValue(2); // Default value = 2

            entity.Property(p => p.KnockOutStartNumber).HasDefaultValue(16); // Default value = 16

            entity
                .Property(p => p.ParticipantResolutionStrategy)
                .HasDefaultValue(ResolutionStrategy.StatBased)
                .HasConversion<string>();

            entity
                .Property(p => p.TournamentType)
                .HasDefaultValue(TournamentType.GroupStage)
                .HasConversion<string>();

            entity.Property(p => p.CurrentState).HasConversion<string>();

            entity
                .Property(p => p.Status)
                .HasDefaultValue(TournamentStatus.Draft)
                .HasConversion<string>();

            //relation config

            entity
                .HasOne(t => t.GameType)
                .WithMany(g => g.Tournaments)
                .HasForeignKey(t => t.GameTypeId);

            entity
                .HasMany(t => t.Participants)
                .WithMany(p => p.Tournaments)
                .UsingEntity(j => j.ToTable("TournamentParticipant"));
        });

        //match type configuration
        modelBuilder.Entity<MatchType>(entity =>
        {
            //matchtype relation config
            entity
                .HasDiscriminator<string>("MatchType")
                .HasValue<Group>("Group")
                .HasValue<KnockOut>("Knockout");

            entity
                .HasMany(m => m.Players)
                .WithMany(p => p.MatchTypes)
                .UsingEntity(j => j.ToTable("MatchTypeParticipants"));

            //* Round configuration of Match type
            entity
                .HasMany(m => m.Rounds)
                .WithOne(r => r.MatchType)
                .HasForeignKey(r => r.MatchTypeId);
        });

        //* Round configuration
        modelBuilder.Entity<Round>(entity =>
        {
            entity.Property(p => p.RoundNumber).IsRequired();
            entity.Property(p => p.StartTime).IsRequired(false);

            //*relation setup
            entity.HasMany(r => r.Matches).WithOne(m => m.Round).HasForeignKey(m => m.RoundId);
        });

        //* Seeded player configuration
        modelBuilder.Entity<SeededPlayer>(entity =>
        {
            entity
                .HasOne(s => s.Player)
                .WithMany(p => p.SeededEntries)
                .HasForeignKey(s => s.PlayerId);

            entity
                .HasOne(s => s.MatchType)
                .WithMany(m => m.SeededPlayers)
                .HasForeignKey(s => s.MatchTypeId);
        });

        //game type configuration
        modelBuilder.Entity<GameType>(entity =>
        {
            entity
                .Property(p => p.Name) //converting enum value to string
                .HasConversion<string>();
        });

        SeedDefaultGameType(modelBuilder);
        SeedDefaultRoleWithClaim(modelBuilder);

        //making all CreatedAt and UpdatedAt filed of all entity that inherit from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .IsRequired();
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property<DateTime>("UpdatedAt")
                    .IsRequired();
            }
        }
    }

    private static void SeedDefaultGameType(ModelBuilder modelBuilder)
    {
        // Some default Seed Data for GameType
        var gameTypes = Enum.GetValues(typeof(GameTypeSupported))
            .Cast<GameTypeSupported>()
            .Select(
                (gameType, index) =>
                    new GameType
                    {
                        Id = index + 1,
                        Name = gameType,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    }
            )
            .ToArray();
        modelBuilder.Entity<GameType>().HasData(gameTypes);
    }

    public override int SaveChanges()
    {
        //set state of tournament during add/creation
        SetTournamentState();
        AddTimeStamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //set state of tournament during add/creation
        SetTournamentState();
        AddTimeStamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimeStamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow; // Current time
            if (entry.State == EntityState.Added)
            {
                //Only add CreatedAt if it not provided(default or null)
                if (entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = now;
                }
            }
            entry.Entity.UpdatedAt = now;
        }
    }

    //setting tournament initial state based on tournament type when creating Tournament
    private void SetTournamentState()
    {
        var entries = ChangeTracker.Entries<Tournament>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CurrentState =
                    entry.Entity.TournamentType == TournamentType.GroupStage
                        ? TournamentState.GroupState
                        : TournamentState.KnockoutState;
            }
        }
    }

    private void SeedDefaultRoleWithClaim(ModelBuilder modelBuilder)
    {
        var defaultRoles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = Role.Admin.ToString(),
                NormalizedName = Role.Admin.ToString().ToUpper(),
            },
            new IdentityRole
            {
                Name = Role.Moderator.ToString(),
                NormalizedName = Role.Moderator.ToString().ToUpper(),
            },
            new IdentityRole
            {
                Name = Role.Player.ToString(),
                NormalizedName = Role.Player.ToString().ToUpper(),
            },
        };

        // Check if roles already exist
        foreach (var role in defaultRoles)
        {
            var existingRole = modelBuilder
                .Entity<IdentityRole>()
                .Metadata.FindPrimaryKey()
                ?.Properties.FirstOrDefault(r => r.Name == role.Name);
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
            if (role.Name == Role.Admin.ToString())
                AddAdminPermission(identityRoleClaim, role);
            if (role.Name == Role.Moderator.ToString())
                AddModeratorPermission(identityRoleClaim, role);
            if (role.Name == Role.Player.ToString())
                AddPlayerPermission(identityRoleClaim, role);
        }
        foreach (var claim in identityRoleClaim)
        {
            Console.WriteLine(
                $"Role ID: {claim.RoleId}, Claim Type: {claim.ClaimType}, Claim Value: {claim.ClaimValue}"
            );
        }

        modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(identityRoleClaim);
    }

    private void AddModeratorPermission(
        List<IdentityRoleClaim<string>> identityRoleClaim,
        IdentityRole role
    )
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 1,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Read,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 2,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Edit,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 3,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.AddScore,
            }
        );
    }

    private void AddAdminPermission(
        List<IdentityRoleClaim<string>> identityRoleClaim,
        IdentityRole role
    )
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 4,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Read,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 5,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Edit,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 6,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Create,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 7,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Delete,
            }
        );
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 8,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.AddScore,
            }
        );
    }

    private void AddPlayerPermission(
        List<IdentityRoleClaim<string>> identityRoleClaim,
        IdentityRole role
    )
    {
        identityRoleClaim.Add(
            new IdentityRoleClaim<string>
            {
                Id = 9,
                RoleId = role.Id,
                ClaimType = DomainClaim.PermissionClaimType,
                ClaimValue = Policy.Read,
            }
        );
    }
}

