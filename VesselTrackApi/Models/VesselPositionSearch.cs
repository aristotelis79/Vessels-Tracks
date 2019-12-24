using System;
using System.Collections.Generic;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Models
{
    /// <summary>
    /// Vessel Position Search Object
    /// </summary>
    [Serializable]
    public class VesselPositionSearch
    {
        public VesselPositionSearch(long[] mmsi, double? minLat, double? maxLat, double? minLon, double? maxLon, DateTime? from, DateTime? to)
        {
            Mmsi = Searchable<IEnumerable<long>>.Create(mmsi);
            Timestamp = Searchable<Between<DateTime?>>.Create(Between<DateTime?>.Create(from, to));
            Latitude = Searchable<Between<double?>>.Create(Between<double?>.Create(minLat,maxLat));
            Longitude = Searchable<Between<double?>>.Create(Between<double?>.Create(minLon,maxLon));
            GeoPoint = Searchable<Between<GeoPoint>>.Create(Between<GeoPoint>.
                Create(new GeoPoint(minLat ?? Coord.MinLat,maxLon ?? Coord.MaxLon),
                    new GeoPoint(maxLat ?? Coord.MaxLat,minLon?? Coord.MinLon)));
        }


        /// <summary>
        /// Search in list of unique vessel identifier
        /// </summary>
        public Searchable<IEnumerable<long>> Mmsi { get; }

        /// <summary>
        /// From and To for search in longitude(between -180, 180)
        /// </summary>
        public Searchable<Between<double?>> Longitude { get;  }

        /// <summary>
        /// From and To for search in latitude(between -90, 90)
        /// </summary>
        public Searchable<Between<double?>> Latitude { get;  }


        /// <summary>
        /// Search in area of GeoPoint
        /// </summary>
        public Searchable<Between<GeoPoint>> GeoPoint { get;  }

        /// <summary>
        /// From and To for search in time (Time convert it to utc)
        /// </summary>
        public Searchable<Between<DateTime?>> Timestamp { get; }
    }
}