using System;
using System.Runtime.Serialization;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Models
{
    /// <summary>
    /// Response Object of search of vessels
    /// </summary>
    [Serializable]
    public class VesselPosition : IJsonApi<Guid>
    {

        public VesselPosition()
        {
            if (Id == Guid.Empty)
                Id = GuidGenerator.GenerateTimeBasedGuid();   
        }

        /// <summary>
        /// Unique Identity of vessel Position
        /// </summary>
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        /// <summary>
        /// position timestamp
        /// </summary>
        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }
        /// <summary>
        /// unique vessel identifier 
        /// </summary>
        [DataMember(Name = "mmsi")]
        public long Mmsi { get; set; }
        /// <summary>
        /// AIS vessel status
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }
        /// <summary>
        /// receiving station ID
        /// </summary>
        [DataMember(Name = "station_id")]
        public int StationId { get; set; }
        /// <summary>
        /// speed in knots x 10 (i.e. 10,1 knots is 101)
        /// </summary>
        [DataMember(Name = "speed")]
        public int Speed { get; set; }
        /// <summary>
        /// longitude
        /// </summary>
        [DataMember(Name = "lon")]
        public double Longitude { get; set; }
        /// <summary>
        /// latitude
        /// </summary>
        [DataMember(Name = "lat")]
        public double Latitude { get; set; }
        /// <summary>
        /// vessel's course over ground
        /// </summary>
        [DataMember(Name = "course")]
        public int Course { get; set; }
        /// <summary>
        /// vessel's true heading
        /// </summary>
        [DataMember(Name = "heading")]
        public int Heading { get; set; }
        /// <summary>
        /// vessel's rate of turn
        /// </summary>
        [DataMember(Name = "rot")]
        public string Rot { get; set; }

    }
}