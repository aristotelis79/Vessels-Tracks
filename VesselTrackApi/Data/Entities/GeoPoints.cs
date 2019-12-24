using System.ComponentModel;
using System.Runtime.Serialization;
using Nest;

namespace VesselTrackApi.Data.Entities
{
    [DisplayName("geoPoint")]
    [DataContract(Name = "geoPoint")]
    public class GeoPoint : GeoLocation, IGeoPoint
    {
        public GeoPoint(double latitude, double longitude) : base(latitude, longitude)
        {
        }
    }
}