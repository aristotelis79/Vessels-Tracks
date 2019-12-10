using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Services
{
    public interface ITrackService
    {
        Task<IEnumerable<VesselPositionEntity>> SearchAsync(Expression<Func<VesselPositionEntity, bool>> filter = null,
            Func<IQueryable<VesselPositionEntity>, IOrderedQueryable<VesselPositionEntity>> orderBy = null,
            string includeProperties = "",
            CancellationToken token = default(CancellationToken));
        
    }
}