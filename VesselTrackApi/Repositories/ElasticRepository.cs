using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using VesselTrackApi.Data.ElasticSearch;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Repositories
{
    public class ElasticRepository<TEntity,T> : IRepository<TEntity,T> where TEntity : class,
                                                IEntity<T> where T : struct 
    {

        private readonly INoSqlDbContext<IElasticClient> _context;

        public ElasticRepository(INoSqlDbContext<IElasticClient> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token)
        {
            var response = await _context.Client.IndexManyAsync(entities, cancellationToken: token).ConfigureAwait(false);
            return response.IsValid 
                ? true 
                : throw new ElasticException(response);
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(ISearchRequest<TEntity> query = null, CancellationToken token = default(CancellationToken))
        {

            var result = (await _context.Client.SearchAsync<TEntity>(query, token)
                .ConfigureAwait(false));
            return result.Documents.AsEnumerable();


        }
    }
}