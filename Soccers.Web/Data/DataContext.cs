using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Soccers.Web.Data.Entities;

namespace Soccers.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<GroupDetailEntity> GroupDetails { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TournamentEntity> Tournaments { get; set; }
        public DbSet<PredictionEntity> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TeamEntity>()
             .HasIndex(t => t.Name)
             .IsUnique();
          
        }

    }
}
