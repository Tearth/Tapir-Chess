using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Tapir.Core.Interfaces;
using Tapir.Providers.EventStore.MongoDB.Implementations;
using Tapir.Core.Domain;

namespace Tapir.Providers.EventStore.MongoDB
{
    public static class Module
    {
        public static IServiceCollection AddMongoDBEventStore(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            services.AddTransient<IMongoClient, MongoClient>(p => new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(configuration.Host, configuration.Port ?? 0),
                Credential = new MongoCredential(
                    configuration.AuthenticationMethod,
                    new MongoInternalIdentity(configuration.DatabaseName, configuration.Username),
                    new PasswordEvidence(configuration.Password))
            }));
            services.AddTransient<IDomainEventStore, DomainEventStore>();
            services.AddSingleton(configuration);

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonClassMap.RegisterClassMap<DomainEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });

            return services;
        }
    }
}
