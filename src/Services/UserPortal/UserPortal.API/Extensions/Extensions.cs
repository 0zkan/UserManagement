using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Models;
using UserManagement.Services.UserPortal.API.Repositories;
using UserManagement.Services.UserPortal.API.Settings;

namespace UserManagement.Services.UserPortal.API.Extensions
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto(user.Name, user.PasswordHash);
        }
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(
                serviceProvider =>
                {
                    var database = serviceProvider.GetService<IMongoDatabase>();
                    return new MongoRepository<T>(database, collectionName);
                }
            );

            return services;
        }
    }
}