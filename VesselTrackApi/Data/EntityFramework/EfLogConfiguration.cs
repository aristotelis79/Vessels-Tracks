using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Data.EntityFramework
{
    public class EfLogConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder
                .ToTable("Log")
                .HasKey(x => x.Id);

            builder.Property(p => p.Application)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Logged)
                .IsRequired();

            builder.Property(p => p.Level)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Message)
                .IsRequired();
            
            builder.Property(p => p.Logger)
                .HasMaxLength(250);
        }
    }
}