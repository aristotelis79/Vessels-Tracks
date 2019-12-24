using System;
using System.Collections.Generic;
using Nest;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Models
{
    public static class SearchRequestBuilder
    {
        public static ISearchRequest<T> SearchRequest<T>(this VesselPositionSearch vps) where T : class, ITimeEntity, IVesselIdentity
        {
            return new SearchRequest<T>
            {
                Size = 10000,
                Query = BetweenGeoBoundingBox(vps.GeoPoint)
                        && BetweenDate<T>(vps.Timestamp) 
                        && In<T>(vps.Mmsi)
            };
        }

        public static QueryContainer BetweenDate<T>(Searchable<Between<DateTime?>> s) where T : class, ITimeEntity
        {
            if (CheckNulls(s)) return null;

            var query = new QueryContainerDescriptor<T>();

            var epochFrom = s?.Value?.From.ToEpoch();
            var queryFrom = query.LongRange(l => l.Field(f => f.Timestamp)
                .GreaterThanOrEquals(epochFrom));

            var epochTo = s?.Value?.To.ToEpoch();
            var queryTo = query.LongRange(l => l.Field(f => f.Timestamp)
                .LessThanOrEquals(epochTo));

            if (epochFrom == null) return queryTo;
            if (epochTo == null) return queryFrom;

            return queryFrom && queryTo;
        }

        public static QueryContainer BetweenGeoBoundingBox(Searchable<Between<GeoPoint>> s)
        {
            if (s == null) return null;

            return new QueryContainer(new GeoBoundingBoxQuery()
            {
                Field = new Field(char.ToLowerInvariant(nameof(GeoPoint)[0]) + nameof(GeoPoint).Substring(1)),
                BoundingBox = new BoundingBox()
                {
                    BottomRight = s.Value.From,
                    TopLeft = s.Value.To
                }
            });
        }

        public static QueryContainer In<T>(Searchable<IEnumerable<long>> s) where T : class, IVesselIdentity
        {
            return s == null 
                ? null 
                : new QueryContainerDescriptor<T>().Terms(t => t.Field(f => f.Mmsi).Terms(s.Value));
        }

        private static bool CheckNulls<T>(Searchable<Between<T?>> s) where T : struct => s?.Value?.From == null && s?.Value?.To == null;

    }
}