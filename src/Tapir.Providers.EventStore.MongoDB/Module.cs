using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Tapir.Core.Domain;
using Tapir.Core.Persistence;
using Tapir.Providers.EventStore.MongoDB.Persistence;
using Tapir.Providers.EventStore.MongoDB.Documents;

namespace Tapir.Providers.EventStore.MongoDB
{
    public static class Module
    {
        public static IServiceCollection AddMongoDBEventStore(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            if (configuration.Servers == null || configuration.Servers.Count == 0)
            {
                throw new InvalidOperationException("No servers configured.");
            }

            services.AddTransient<IMongoClient, MongoClient>(p => new MongoClient(new MongoClientSettings
            {
                Servers = configuration.Servers.Select(s => new MongoServerAddress(s.Host, s.Port ?? 0)),
                Credential = new MongoCredential(
                    configuration.AuthenticationMethod,
                    new MongoInternalIdentity(configuration.DatabaseName, configuration.Username),
                    new PasswordEvidence(configuration.Password)),
            }));
            services.AddTransient<IDomainEventStore, DomainEventStore>();
            services.AddSingleton(configuration);

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonClassMap.RegisterClassMap<DomainEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });
            BsonClassMap.RegisterClassMap<AggregateDocument>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });

            return services;
        }
    }
}
