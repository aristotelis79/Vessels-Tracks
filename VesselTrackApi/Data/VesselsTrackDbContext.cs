using Microsoft.EntityFrameworkCore;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Data
{
    public class VesselsTrackDbContext : DbContext, IDbContext
    {
        public DbSet<VesselPositionEntity> VesselPositionEntities { get; set; }
        public readonly string _connectionString;

        public VesselsTrackDbContext(string connectionString) : base()
        {
            this._connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        public VesselsTrackDbContext(DbContextOptions<VesselsTrackDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntity>()
                        .ToTable("Log")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<LogEntity>()
                        .Property(p => p.Application)
                        .HasMaxLength(50)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(p => p.Logged)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(p => p.Level)
                        .HasMaxLength(50)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(p => p.Message)
                        .IsRequired();
            
            modelBuilder.Entity<LogEntity>()
                        .Property(p => p.Logger)
                        .HasMaxLength(250);

            modelBuilder.Entity<VesselPositionEntity>()
                        .ToTable("VesselPosition")
                        .HasKey(p =>  p.Id );

            modelBuilder.Entity<VesselPositionEntity>()
                        .Property(p => p.Lon)
                        .HasPrecision(8, 5);

            modelBuilder.Entity<VesselPositionEntity>()
                        .Property(p => p.Lat)
                        .HasPrecision(8, 5);

            modelBuilder.Entity<VesselPositionEntity>()
                        .HasIndex(p => p.Mmsi)
                        .HasName("idx_VesselPosition_mmsi");

            modelBuilder.Entity<VesselPositionEntity>()
                        .HasIndex(p => p.Timestamp)
                        .HasName("idx_VesselPosition_timestamp");


            modelBuilder.Entity<VesselPositionEntity>()
                        .HasIndex(p => new { p.Lat , p.Lon})
                        .HasName("idx_VesselPosition_lat_lon");

        }

        public DbSet<TEntity> Set<TEntity, T>() where TEntity : class, IEntity<T> where T : struct
        {
            return base.Set<TEntity>();
        }
    }
}