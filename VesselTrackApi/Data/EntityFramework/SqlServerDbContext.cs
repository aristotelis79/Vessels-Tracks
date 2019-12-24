using Microsoft.EntityFrameworkCore;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Data.EntityFramework
{
    public class SqlServerDbContext : DbContext, IDbContext
    {
        public DbSet<VesselPositionEntity> VesselPositionEntities { get; set; }
        public readonly string _connectionString;

        public SqlServerDbContext(string connectionString) : base()
        {
            this._connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new EfLogConfiguration());
            modelBuilder.ApplyConfiguration(new EfVesselPositionConfiguration());

        }

        public DbSet<TEntity> Set<TEntity, T>() where TEntity : class, IEntity<T> where T : struct
        {
            return base.Set<TEntity>();
        }
    }
}