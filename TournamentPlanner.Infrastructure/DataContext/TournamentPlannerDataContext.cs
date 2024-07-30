using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Infrastructure.DataContext
{
    public class TournamentPlannerDataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<KnockOut> KnockOuts { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public TournamentPlannerDataContext(DbContextOptions<TournamentPlannerDataContext> options) : base(options)
        { }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(250);

                entity.Property(p => p.Email)
                .IsRequired()
                .HasAnnotation("RegularExpression", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"); //Basic regex for email
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(250);
                entity.Property(p => p.PhoneNumber)
                      .IsRequired();
                entity.Property(p => p.Email)
                .IsRequired()
                .HasAnnotation("RegularExpression", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"); //Basic regex for email
            });

            //* Database provider dont support Player type exception
            // modelBuilder.Entity<Match>(entity => {
            //     entity.Property(p => p.FirstPlayer).IsRequired();
            //     entity.Property(p => p.SecondPlayer).IsRequired();
            // });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.Property(p => p.FirstPlayer)
                      .IsRequired();
                entity.Property(p => p.SecondPlayer)
                      .IsRequired();
                entity.Property(p => p.IsRescheduled)
                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(p => p.Name)
                      .IsRequired();
            });

            modelBuilder.Entity<KnockOut>(entity =>
            {
                entity.Property(p => p.Name)
                      .IsRequired();
                entity.Property(p => p.Round)
                      .IsRequired();
            });

            modelBuilder.Entity<GameType>(entity =>
            {
                entity.Property(p => p.Name)
                      .IsRequired();
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(p => p.Name)
                      .HasMaxLength(250)
                      .IsRequired();
                entity.Property(p => p.StartDate)
                      .IsRequired();
                entity.Property(p => p.MaxParticipant)
                      .HasDefaultValue<int>(104); // 26 Group (A-Z) * 4 per group = 104
                entity.Property(p => p.RegistrationFee)
                      .HasColumnType("decimal(18,2)");
                entity.Property(p => p.MinimumAgeOfRegistration)
                      .HasDefaultValue<int>(18);
                entity.Property(p => p.WinnerPerGroup)
                      .HasDefaultValue(2); // Default value = 2
                entity.Property(p => p.KnockOutStartNumber)
                      .HasDefaultValue(16); // Default value = 16
                entity.Property(p => p.ParticipantResolutionStrategy)
                      .HasDefaultValue(ResolutionStrategy.StatBased)
                      .HasConversion<string>();
                entity.Property(p => p.TournamentType)
                      .HasDefaultValue(TournamentType.GroupStage)
                      .HasConversion<string>();
                entity.Property(p => p.GameType)
                      .IsRequired();
                entity.Property(p => p.Status)
                      .HasDefaultValue(TournamentStatus.Draft)
                      .HasConversion<string>();

            });

            //making all CreatedAt and UpdatedAt filed of all entity that inherit from BaseEntity
            //TODO: Can do similar thing for player and admin on name and email field
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

    }
}