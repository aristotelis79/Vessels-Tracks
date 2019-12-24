using System;
using Nest;

namespace VesselTrackApi.Repositories
{
    public class ElasticException : Exception
    {
        public ElasticException()
        {
        }

        public ElasticException(ResponseBase response)
            : base($" Error: {response.ServerError.Error}, DebugInformation: ${response.DebugInformation}")
        {
        }
    }
}