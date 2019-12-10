using System;
using System.Collections.Generic;
using System.Linq;

namespace VesselTrackApi.Models
{
    /// <summary>
    /// Vessel Position Search Object
    /// </summary>
    [Serializable]
    public class VesselPositionSearch
    {
        public VesselPositionSearch(long[] mmsi, decimal? minLat, decimal? maxLat, decimal? minLon, decimal? maxLon, DateTime? from, DateTime? to)
        {
            Mmsi = mmsi != null && mmsi.Any() ?  new Searchable<IList<long>>(mmsi.ToList()) : null;
            Lat = minLat != null || maxLat != null  ? new Searchable<Between<decimal?>>(new Between<decimal?>(minLat,maxLat)) : null;
            Lon = minLon != null || maxLon != null  ? new Searchable<Between<decimal?>>(new Between<decimal?>(minLon,maxLon)) : null;
            Timestamp =  from != null || to != null  ? new Searchable<Between<DateTime?>>(new Between<DateTime?>(from, to)) : null;
        }

        /// <summary>
        /// Search in list of unique vessel identifier
        /// </summary>
        public Searchable<IList<long>> Mmsi { get;  }

        /// <summary>
        /// From and To for search in longitude(between -180, 180)
        /// </summary>
        public Searchable<Between<decimal?>> Lon { get;  }

        /// <summary>
        /// From and To for search in latitude(between -90, 90)
        /// </summary>
        public Searchable<Between<decimal?>> Lat { get;  }

        /// <summary>
        /// From and To for search in time (Time convert it to utc)
        /// </summary>
        public Searchable<Between<DateTime?>> Timestamp { get; }
    }
}