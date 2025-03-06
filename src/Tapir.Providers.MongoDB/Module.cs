using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tapir.Core.Interfaces;
using Tapir.Providers.MongoDB.Implementations;

namespace Tapir.Providers.MongoDB
{
    public static class Module
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            services.AddTransient<IMongoClient, MongoClient>(p => new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(configuration.Host, configuration.Port),
                Credential = new MongoCredential(
                    configuration.AuthenticationMethod,
                    new MongoInternalIdentity(configuration.DatabaseName, configuration.Username),
                    new PasswordEvidence(configuration.Password))
            }));
            services.AddTransient<IEventStorage, EventStorage>();
            services.AddSingleton(configuration);

            return services;
        }
    }
}
