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
                Latitude = entity.GeoPoint.Latitude,
                Longitude = entity.GeoPoint.Longitude,
                Heading = entity.Heading,
                Timestamp = entity.Timestamp

            };
        }


        public static List<VesselPosition> ToVesselPositions(this IEnumerable<VesselPositionEntity> entities) => entities.Select(e => e.ToVesselPosition()).ToList();
        

        public static VesselPositionEntity ToVesselPositionEntity(this VesselPosition model)
        {
            return new VesselPositionEntity
            {
                Id = model.Id,
                Mmsi = model.Mmsi,
                StationId = model.StationId,
                Status = model.Status,
                Speed = model.Speed,
                Course = model.Course,
                Heading = model.Heading,
                Timestamp = model.Timestamp,
                GeoPoint = new GeoPoint(model.Latitude, model.Longitude)
            };
        }

        public static List<VesselPositionEntity> ToVesselPositionEntities(this IEnumerable<VesselPosition> models) => models.Select(e => e.ToVesselPositionEntity()).ToList();

    }
}