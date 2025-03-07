using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Reflection;
using Tapir.Core.Domain;
using Tapir.Core.Interfaces;

namespace Tapir.Providers.MongoDB.Implementations
{
    public class EventStorage : IDomainEventStorage
    {
        private Configuration _configuration;
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IDomainEventRegistry _eventRegistry;

        private const string EventsCollectionName = "events";

        public EventStorage(Configuration configuration, IMongoClient mongoClient, IDomainEventRegistry eventRegistry)
        {
            _configuration = configuration;
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(_configuration.DatabaseName);
            _eventRegistry = eventRegistry;
        }

        public async Task AddAsync<T>(T @event) where T : DomainEvent
        {
            await _mongoDatabase.GetCollection<T>(EventsCollectionName).InsertOneAsync(@event);
        }

        public async Task<IEnumerable<DomainEvent>> GetByStreamGuid(Guid streamGuid)
        {
            var events = new List<DomainEvent>();
            var filter = Builders<BsonDocument>.Filter.Eq("StreamGuid", streamGuid);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            foreach (var document in documents)
            {
                var type = document["Type"].AsString;
                var assemblyType = _eventRegistry.GetAssemblyType(streamGuid, type);

                events.Add((DomainEvent)BsonSerializer.Deserialize(document, assemblyType));
            }

            return events;
        }
    }
}
