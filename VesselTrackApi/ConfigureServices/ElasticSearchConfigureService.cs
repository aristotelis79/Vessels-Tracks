using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Repositories;
using VesselTrackApi.Data.ElasticSearch;

namespace VesselTrackApi.ConfigureServices
{
    public static class ElasticSearchConfigureService
    {

        public static IServiceCollection AddElasticDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionSettings = new ConnectionSettings(new Uri(configuration["ElasticSearch:url"]))
                                                    .EnableHttpCompression()
                                                    //.BasicAuthentication(configuration["ElasticSearch:username"],
                                                    //                    configuration["ElasticSearch:password"])
#if DEBUG
                                                    .EnableDebugMode()
                                                    .DisableDirectStreaming()
                                                    .PrettyJson()  
#endif
                                                    .DefaultIndex(configuration["ElasticSearch:index"]);

            var elasticDbContext = new ElasticDbContext(connectionSettings);

            services.AddScoped<IRepository<VesselPositionEntity,Guid>, ElasticRepository<VesselPositionEntity,Guid>>();
            services.AddSingleton<INoSqlDbContext<IElasticClient>>( x => elasticDbContext);
            services.AddSingleton<ConnectionSettings>( x => connectionSettings);
            services.AddSingleton<IElasticClient>(c => elasticDbContext.Client);

            return services;
        }
    }
}