using MongoDB.Driver;
using Tapir.Core.Interfaces;

namespace Tapir.Providers.MongoDB.Implementations
{
    public class EventStorage : IEventStorage
    {
        private Configuration _configuration;
        private IMongoClient _mongoClient;

        public EventStorage(Configuration configuration, IMongoClient mongoClient)
        {
            _configuration = configuration;
            _mongoClient = mongoClient;
        }

        public Task AddAsync<T, D>(T @event) where T : IEvent<D>
        {
            throw new NotImplementedException();
        }
    }
}
