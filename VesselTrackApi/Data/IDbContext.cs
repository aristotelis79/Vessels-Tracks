using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Repositories;

namespace VesselTrackApi.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity,T>() where TEntity : class, IEntity<T> where T : struct;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}