using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Repositories
{
    public partial class EfRepository<TEntity,T> : IRepository<TEntity,T> where TEntity : class,
                                                    IEntity<T> where T : struct 
    {
        private readonly IDbContext _context;

        private DbSet<TEntity> _entities;

        public EfRepository(IDbContext context)
        {
            _context = context;
        }


        public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken token)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Entities.AddRangeAsync(entities, token).ConfigureAwait(false);
                await _context.SaveChangesAsync(token).ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(await GetFullErrorTextAndRollbackEntityChanges(exception, token).ConfigureAwait(false), exception);
            }
        }

        public virtual IQueryable<TEntity> Table => Entities;

        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = _context.Set<TEntity,T>());


        protected async Task<string> GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception, CancellationToken token)
        {
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }
            
            try
            {
                await _context.SaveChangesAsync(token).ConfigureAwait(false);
                return exception.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString(); 
            }
        }
    }
}