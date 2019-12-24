using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Repositories
{
    public partial interface IRepository<TEntity,T> where TEntity : class, IEntity<T> where T : struct
    {

        Task<bool> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token);

        Task<IEnumerable<TEntity>> SearchAsync(ISearchRequest<TEntity> query = null,
                                                                CancellationToken token = default(CancellationToken));
    }
}