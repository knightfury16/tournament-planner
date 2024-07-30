using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Infrastructure.DataContext
{
    public class TournamentPlannerDataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Round> Rounds { get; set; }
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

            modelBuilder.Entity<Round>(entity =>
            {
                entity.Property(p => p.RoundNumber).IsRequired();
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(p => p.Name).IsRequired();
            });

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