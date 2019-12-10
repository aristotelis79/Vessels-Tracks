using System;
using Newtonsoft.Json;

namespace VesselTrackApi.Models
{
    /// <summary>
    /// Response Object of search of vessels
    /// </summary>
    [Serializable]
    public class VesselPosition : IJsonApi
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        /// <summary>
        /// position timestamp
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
        /// <summary>
        /// unique vessel identifier 
        /// </summary>
        [JsonProperty(PropertyName = "mmsi")]
        public long Mmsi { get; set; }
        /// <summary>
        /// AIS vessel status
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        /// <summary>
        /// receiving station ID
        /// </summary>
        [JsonProperty(PropertyName = "station_id")]
        public int StationId { get; set; }
        /// <summary>
        /// speed in knots x 10 (i.e. 10,1 knots is 101)
        /// </summary>
        [JsonProperty(PropertyName = "speed")]
        public int Speed { get; set; }
        /// <summary>
        /// longitude
        /// </summary>
        [JsonProperty(PropertyName = "lon")]
        public decimal Lon { get; set; }
        /// <summary>
        /// latitude
        /// </summary>
        [JsonProperty(PropertyName = "lat")]
        public decimal Lat { get; set; }
        /// <summary>
        /// vessel's course over ground
        /// </summary>
        [JsonProperty(PropertyName = "course")]
        public int Course { get; set; }
        /// <summary>
        /// vessel's true heading
        /// </summary>
        [JsonProperty(PropertyName = "heading")]
        public int Heading { get; set; }
        /// <summary>
        /// vessel's rate of turn
        /// </summary>
        [JsonProperty(PropertyName = "rot")]
        public string Rot { get; set; }

    }
}