using System;
using System.Linq;

namespace VesselTrackApi.SwaggerHelpers
{
    public static class ApiDocHelper
    {
        public static string DefaultSchemaIdSelector( this Type modelType)
        {
            if (!modelType.IsConstructedGenericType) return modelType.Name;

            var prefix = modelType.GetGenericArguments()
                .Select(DefaultSchemaIdSelector)
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }
    }
}