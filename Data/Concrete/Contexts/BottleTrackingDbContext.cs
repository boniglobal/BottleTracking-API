using Data.Concrete.Contexts.Mapping;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Concrete.Contexts
{
    public class BottleTrackingDbContext : DbContext
    {
        public BottleTrackingDbContext(DbContextOptions<BottleTrackingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BottleMap());
            modelBuilder.ApplyConfiguration(new StationMap());
            modelBuilder.ApplyConfiguration(new StationLogMap());
            modelBuilder.ApplyConfiguration(new ErrorLogMap());
            modelBuilder.ApplyConfiguration(new PanelUserMap());
            modelBuilder.ApplyConfiguration(new RefreshTokenMap());
        }

        public DbSet<Bottle> Bottles { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationLog> StationLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<PanelUser> PanelUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
