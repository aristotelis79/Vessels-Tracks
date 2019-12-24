using Nest;
using VesselTrackApi.Data.Entities;

namespace VesselTrackApi.Data.ElasticSearch
{
    public class ElasticDbContext : INoSqlDbContext<IElasticClient>
    {
        public IElasticClient Client { get; }

        public ElasticDbContext(ConnectionSettings settings)
        {
            this.Client = new ElasticClient(settings);

            settings.DefaultMappingFor<VesselPositionEntity>(m => m
                                                            //.IndexName(configuration["ElasticSearch:index"])
                                                            .IdProperty(p => p.Id)
                                                            .PropertyName(p => p.Id, "id"));

            CreateIndexesAsync(this.Client, "vessel_positions");
        }



        public static void CreateIndexesAsync(IElasticClient client, string indexName)
        {
            client.Indices.CreateAsync(indexName, s =>
                                                   s.Map<VesselPositionEntity>(m =>
                                                       m.Properties(p => p
                                                           .Keyword(x => x.Name(n => n.Id))
                                                           .Number(x => x.Name(n => n.Mmsi).Type(NumberType.Long))
                                                           .Number(x => x.Name(n => n.StationId).Type(NumberType.Integer).Index(false))
                                                           .Number(x => x.Name(n => n.Status).Type(NumberType.Integer).Index(false))
                                                           .Number(x => x.Name(n => n.Heading).Type(NumberType.Integer).Index(false))
                                                           .Number(x => x.Name(n => n.Course).Type(NumberType.Integer).Index(false))
                                                           .Number(x => x.Name(n => n.Speed).Type(NumberType.Integer).Index(false))
                                                           .GeoPoint(x => x.Name(n => n.GeoPoint).IgnoreZValue())
                                                           .Number(x => x.Name(n => n.Timestamp).Type(NumberType.Long))
                                                           .Text(x => x.Name(n => n.Rot).Index(false)))));
        }
    }
}