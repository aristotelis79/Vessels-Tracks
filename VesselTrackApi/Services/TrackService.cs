using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Repositories;

namespace VesselTrackApi.Services
{
    public class TrackService : ITrackService
    {
        private readonly IRepository<VesselPositionEntity,long> _repository;

        public TrackService(IRepository<VesselPositionEntity,long> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public async Task<IEnumerable<VesselPositionEntity>> SearchAsync(Expression<Func<VesselPositionEntity, bool>> filter = null,
                                                        Func<IQueryable<VesselPositionEntity>, IOrderedQueryable<VesselPositionEntity>> orderBy = null,
                                                        string includeProperties = "",
                                                        CancellationToken token = default(CancellationToken))
        {
            var query = _repository.TableNoTracking;

            if (filter != null)
            {
                query =  query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            

            return orderBy != null 
                ? await orderBy(query).ToListAsync(cancellationToken: token).ConfigureAwait(false) 
                : await query.ToListAsync(cancellationToken: token).ConfigureAwait(false) ;
        }
    }
}