```c#
public class TournamentPlannerDataContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<KnockOut> KnockOuts { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<GameType> GameTypes { get; set; }

    public TournamentPlannerDataContext(DbContextOptions<TournamentPlannerDataContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(250);
            entity.Property(p => p.Email)
                  .IsRequired();
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(250);
            entity.Property(p => p.Email)
                  .IsRequired();
            entity.Property(p => p.PhoneNumber)
                  .IsRequired();
        });

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

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(p => p.Name)
                  .IsRequired();
            entity.Property(p => p.StartDate)
                  .IsRequired();
            entity.Property(p => p.MaxParticipant)
                  .IsRequired();
            entity.Property(p => p.RegistrationFee)
                  .HasColumnType("decimal(18,2)");
            entity.Property(p => p.MinimumAgeOfRegistration)
                  .IsRequired();
            entity.Property(p => p.WinnerPerGroup)
                  .IsRequired();
            entity.Property(p => p.KnockOutStartNumber)
                  .IsRequired();
            entity.Property(p => p.ParticipantResolutionStrategy)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(p => p.TournamentType)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(p => p.GameType)
                  .IsRequired();
            entity.Property(p => p.Status)
                  .IsRequired()
                  .HasDefaultValue(TournamentStatus.Draft)
                  .HasConversion<string>();
                  
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.Property(p => p.Name)
                  .IsRequired();
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

```