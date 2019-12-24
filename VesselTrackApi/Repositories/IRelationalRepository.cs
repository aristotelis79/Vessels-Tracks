using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Repositories
{
    public partial interface IRelationalRepository<TEntity,T> where TEntity : IEntity<T> where T : struct
    {

        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken token);

        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            CancellationToken token = default(CancellationToken));

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }
    }
}