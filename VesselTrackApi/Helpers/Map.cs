using System.Collections.Generic;
using System.Linq;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Models;

namespace VesselTrackApi.Helpers
{
    public static class Map
    {
        public static VesselPosition ToVesselPosition(this VesselPositionEntity entity)
        {
            return new VesselPosition
            {
                Id = entity.Id,
                Mmsi = entity.Mmsi,
                StationId = entity.StationId,
                Status = entity.Status,
                Speed = entity.Speed,
                Course = entity.Course,
                Lat = entity.Lat,
                Lon = entity.Lon,
                Heading = entity.Heading,
                Timestamp = entity.Timestamp

            };
        }


        public static List<VesselPosition> ToVesselPositions(this IEnumerable<VesselPositionEntity> entities) => entities.Select(e => e.ToVesselPosition()).ToList();
        
    }
}