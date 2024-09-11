namespace TournamentPlanner.DataModeling;

using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

public class TournamentPlannerDataContext : DbContext
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

    public TournamentPlannerDataContext(DbContextOptions<TournamentPlannerDataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(p => p.Age).IsRequired();
            entity.Property(p => p.Weight).IsRequired();
        });

        // Admin configuration
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(a => a.PhoneNumber).IsRequired();
        });


        // Match configuration
        modelBuilder.Entity<Match>(entity =>
        {
            // fields
            entity.Property(m => m.ScoreJson).HasColumnType("jsonb");

            entity.HasOne(m => m.FirstPlayer)
            .WithMany()
            .HasForeignKey(m => m.Player1Id);

            entity.HasOne(m => m.SecondPlayer)
            .WithMany()
            .HasForeignKey(m => m.Player2Id);

            entity.HasOne(m => m.Winner)
            .WithMany()
            .HasForeignKey(m => m.WinnerId);

            entity.HasOne(m => m.RescheduledBy)
            .WithOne()
            .HasForeignKey<Match>(m => m.RescheduledById);

            entity.HasOne(m => m.Round)
                .WithMany(r => r.Matches)
                .HasForeignKey(m => m.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        //tournament configuration
        modelBuilder.Entity<Tournament>(entity =>
        {
            //filed config
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

            entity.Property(p => p.Status)
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
            entity.HasMany(r => r.Matches)
            .WithOne(m => m.Round)
            .HasForeignKey(m => m.RoundId);
        });


        //game type configuration
        modelBuilder.Entity<GameType>(entity =>
        {
            entity.Property(p => p.Name) //converting enum value to string
            .HasConversion<string>();

        });

        // Some default Seed Data for GameType
        var gameTypes = Enum.GetValues(typeof(GameTypeSupported)).Cast<GameTypeSupported>()
                        .Select((gameType, index) => new GameType
                        {
                            Id = index + 1,
                            Name = gameType,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        }).ToArray();
        modelBuilder.Entity<GameType>().HasData(gameTypes);

        //making all CreatedAt and UpdatedAt filed of all entity that inherit from BaseEntity
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