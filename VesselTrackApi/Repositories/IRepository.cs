using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Repositories
{
    public partial interface IRepository<TEntity,T> where TEntity : IEntity<T> where T : struct
    {

        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken token);

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }
    }
}