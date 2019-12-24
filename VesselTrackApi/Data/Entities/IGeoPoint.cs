using System;
using Nest;

namespace VesselTrackApi.Data.Entities
{
    public interface IGeoPoint : IEquatable<GeoLocation>, IFormattable
    {
        double Latitude { get; }

        double Longitude { get; }

    }
}