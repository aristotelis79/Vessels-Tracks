using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace VesselTrackApi.Data.Entities
{
    [Serializable]
    public class VesselPositionEntity : IEntity<long>, ITimeEntity, IPosition, IVesselIdentity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("timestamp")]
        public long Timestamp { get; set; }

        [Column("mmsi")]
        public long Mmsi { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("station_id")]
        public int StationId { get; set; }

        [Column("speed")]
        public int Speed { get; set; }

        [Column("lon")]
        public decimal Lon { get; set; }

        [Column("lat")]
        public decimal Lat { get; set; }

        [Column("course")]
        public int Course { get; set; }

        [Column("heading")]
        public int Heading { get; set; }

        [Column("rot")]
        public string Rot { get; set; }

    }
}