using System;
using System.Runtime.Serialization;
using Nest;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Data.Entities
{
    [Serializable]
    public class VesselPositionEntity : IEntity<Guid>, ITimeEntity, IVesselIdentity
    {
        public VesselPositionEntity()
        {
            if (Id == Guid.Empty)
                Id = GuidGenerator.GenerateTimeBasedGuid();
        }

        public Guid Id { get; set; }

        public long Timestamp { get; set; }

        public long Mmsi { get; set; }

        public int Status { get; set; }

        public int StationId { get; set; }

        public int Speed { get; set; }

        public virtual GeoLocation GeoPoint { get; set; }

        public int Course { get; set; }

        public int Heading { get; set; }

        public string Rot { get; set; }

    }
}