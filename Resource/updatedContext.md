This example demonstrates how the data will be structured and saved in the database based on the updated data context. Let's break down the key points:

Users Table:

Uses a single table for both Players and Admins (Table-Per-Hierarchy approach).
The UserType column distinguishes between Players and Admins.
Player-specific fields (Age, Weight, etc.) are nullable to accommodate Admin records.


GameFormats Table:

Stores different game types (e.g., Table Tennis, Chess).


Tournaments Table:

Has a foreign key relationship with GameFormats.
Stores tournament-specific information like start date, end date, and rules.


TournamentParticipants Table:

Represents the many-to-many relationship between Tournaments and Players.


Groups Table:

Represents groups within a tournament.


GroupPlayers Table:

Represents the many-to-many relationship between Groups and Players.


Matches Table:

Stores individual match information, including players, winner, and score.
The Score column uses a JSON format to accommodate different scoring systems.



Key points about the data:

Different User Types:

Players (John Doe, Jane Smith) have additional attributes like Age and Weight.
Admin (Admin User) has a PhoneNumber attribute.


Game Types:

Two game types are created: Table Tennis and Chess.
Each tournament is associated with a specific game type.


Tournaments:

Two tournaments are created, one for Table Tennis and one for Chess.


Groups and Matches:

Groups are created for each tournament.
A sample match is inserted for the Table Tennis tournament.


Relationships:

Players are associated with tournaments through the TournamentParticipants table.
Players are associated with groups through the GroupPlayers table.


Score Storage:

The match score is stored as a JSON string, allowing for flexible score representation across different game types.



This structure allows for:

Efficient querying of tournament information, including participants and game types.
Flexibility in handling different user types (Players and Admins) in a single table.
Scalability to add new game types and tournaments.
Detailed tracking of matches, groups, and tournament progress.

To use this in your application:

Ensure your entity classes in the domain layer match this structure.
Use Entity Framework migrations to create and update the database schema.
When saving data, EF Core will handle the insertion into the appropriate tables based on the relationships defined in your data context.

This structure should provide a solid foundation for your tournament planner application, accommodating different user types and game formats while maintaining data integrity and query efficiency.

```c#
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.GameTypesSupported;

namespace TournamentPlanner.Infrastructure.DataContext
{
    public class TournamentPlannerDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<GameFormat> GameFormats { get; set; }

        public TournamentPlannerDataContext(DbContextOptions<TournamentPlannerDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Player>("Player")
                .HasValue<Admin>("Admin");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name).IsRequired().HasMaxLength(250);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(250)
                    .HasAnnotation("RegularExpression", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            });

            // Player configuration
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(p => p.Age).IsRequired();
                entity.Property(p => p.Weight).IsRequired();
                entity.Property(p => p.TournamentsParticipated).IsRequired();
                entity.Property(p => p.GamesPlayed).IsRequired();
                entity.Property(p => p.GamesWon).IsRequired();
            });

            // Admin configuration
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(a => a.PhoneNumber).IsRequired();
            });

            // Tournament configuration
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(t => t.Name).IsRequired().HasMaxLength(250);
                entity.Property(t => t.StartDate).IsRequired();
                entity.Property(t => t.EndDate).IsRequired();
                entity.Property(t => t.MaxParticipants).HasDefaultValue(104);
                entity.Property(t => t.RegistrationFee).HasColumnType("decimal(18,2)");
                entity.Property(t => t.MinimumAgeOfRegistration).HasDefaultValue(18);
                entity.Property(t => t.WinnersPerGroup).HasDefaultValue(2);
                entity.Property(t => t.KnockoutStartNumber).HasDefaultValue(16);
                entity.Property(t => t.ParticipantResolutionStrategy)
                    .HasDefaultValue(ResolutionStrategy.StatBased)
                    .HasConversion<string>();
                entity.Property(t => t.TournamentType)
                    .HasDefaultValue(TournamentType.GroupStage)
                    .HasConversion<string>();
                entity.Property(t => t.Status)
                    .HasDefaultValue(TournamentStatus.Draft)
                    .HasConversion<string>();

                entity.HasOne(t => t.GameFormat)
                    .WithMany()
                    .HasForeignKey("GameFormatId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.Participants)
                    .WithMany(p => p.Tournaments)
                    .UsingEntity(j => j.ToTable("TournamentParticipants"));
            });

            // Match configuration
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(m => m.Player1)
                    .WithMany()
                    .HasForeignKey("Player1Id")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Player2)
                    .WithMany()
                    .HasForeignKey("Player2Id")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Winner)
                    .WithMany()
                    .HasForeignKey("WinnerId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(m => m.Score).HasColumnType("json");

                entity.Property(m => m.IsRescheduled).HasDefaultValue(false);
            });

            // Group configuration
            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(g => g.Name).IsRequired().HasMaxLength(250);

                entity.HasMany(g => g.Players)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("GroupPlayers"));

                entity.HasMany(g => g.Matches)
                    .WithOne()
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // GameFormat configuration
            modelBuilder.Entity<GameFormat>(entity =>
            {
                entity.Property(gf => gf.Name).IsRequired().HasMaxLength(250);
            });

            // Apply common configurations for all entities inheriting from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedAt").IsRequired();
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime>("UpdatedAt").IsRequired();
                }
            }
        }

        public override int SaveChanges()
        {
            AddTimeStamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimeStamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimeStamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreatedAt == default)
                    {
                        entry.Entity.CreatedAt = now;
                    }
                }
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}
```