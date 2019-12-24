using System;

namespace VesselTrackApi.Data.ElasticSearch
{
    public interface INoSqlDbContext<out T> where T : class
    {
        T Client { get; }
    }
}