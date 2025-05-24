using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abstraction;
using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoServices(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            
            services.AddSingleton(serviceProvider =>
            {
                //request a registered service from service provider
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongodbSettings = configuration.GetSection(MongoDbOptions.MongoDbSettings).Get<MongoDbOptions>();
                var mongoClient = new MongoClient(mongodbSettings?.ConnectionString);
                //use the serviceName as the name of the database in mongodb
                var database = mongoClient.GetDatabase(serviceSettings?.ServiceName);
                return database;
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryMongoDB<>));

            return services;
        }
    }
}
