using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Infrastructure.DataContext
{
    public class TournamentPlannerDataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public TournamentPlannerDataContext(DbContextOptions<TournamentPlannerDataContext> options) : base(options)
        {}
        override protected void OnModelCreating(ModelBuilder modelBuilder){

            modelBuilder.Entity<Player>(entity => {
                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(250);
            });

            //* Database provider dont support Player type exception
            // modelBuilder.Entity<Match>(entity => {
            //     entity.Property(p => p.FirstPlayer).IsRequired();
            //     entity.Property(p => p.SecondPlayer).IsRequired();
            // });

            modelBuilder.Entity<Round>(entity => {
                entity.Property(p => p.RoundNumber).IsRequired();
            });

            modelBuilder.Entity<Tournament>(entity => {
                entity.Property(p => p.Name).IsRequired();
            });


            foreach (var entityType in modelBuilder.Model.GetEntityTypes()){

                if(typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)){
                    
                    modelBuilder.Entity(entityType.ClrType).Property("CreatedAt").IsRequired();
                }

            }

        }

        public override int SaveChanges()
        {
            AddTimeStamp();
            return base.SaveChanges();
        }

        private void AddTimeStamp()
        {
            throw new NotImplementedException();
        }
    }
}