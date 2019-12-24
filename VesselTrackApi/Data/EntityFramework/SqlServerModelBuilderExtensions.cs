using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VesselTrackApi.Data.EntityFramework
{
    public static class SqlServerModelBuilderExtensions
    {
        public static PropertyBuilder<double> HasPrecision(this PropertyBuilder<double> builder, int precision, int scale)
        {
            return builder.HasColumnType($"double({precision},{scale})");
        }
    }
}