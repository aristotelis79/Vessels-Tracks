using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Data.EntityFramework
{
    public class EfVesselPositionConfiguration : IEntityTypeConfiguration<VesselPositionEntity>
    {
        public void Configure(EntityTypeBuilder<VesselPositionEntity> builder)
        {
            builder.ToTable("VesselPosition")
                    .HasKey(x => x.Id);

            builder.Property(p => p.GeoPoint.Latitude)
                .HasPrecision(8, 5);

            builder.Property(p => p.GeoPoint.Longitude)
                .HasPrecision(8, 5);

            builder.HasIndex(p => p.Mmsi)
                .HasName("idx_VesselPosition_mmsi");

            builder.HasIndex(p => p.Timestamp)
                .HasName("idx_VesselPosition_timestamp");

            builder.HasIndex(p => new { Lat = p.GeoPoint.Latitude , Lon = p.GeoPoint.Longitude})
                .HasName("idx_VesselPosition_lat_lon");
        }
    }
}