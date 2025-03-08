using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Reflection;
using Tapir.Core.Domain;
using Tapir.Core.Interfaces;

namespace Tapir.Providers.MongoDB.Implementations
{
    public class DomainEventStorage : IDomainEventStorage
    {
        private Configuration _configuration;
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IDomainEventRegistry _eventRegistry;

        private const string EventsCollectionName = "events";

        public DomainEventStorage(Configuration configuration, IMongoClient mongoClient, IDomainEventRegistry eventRegistry)
        {
            _configuration = configuration;
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(_configuration.DatabaseName);
            _eventRegistry = eventRegistry;
        }

        public async Task AddAsync(DomainEvent @event)
        {
            await _mongoDatabase.GetCollection<DomainEvent>(EventsCollectionName).InsertOneAsync(@event);
        }

        public async Task<IEnumerable<DomainEvent>> GetByStreamGuid(Guid streamId)
        {
            var events = new List<DomainEvent>();
            var filter = Builders<BsonDocument>.Filter.Eq("StreamId", streamId);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            foreach (var document in documents)
            {
                var type = document["Type"].AsString;
                var assemblyType = _eventRegistry.GetAssemblyType(streamId, type);

                events.Add((DomainEvent)BsonSerializer.Deserialize(document, assemblyType));
            }

            return events;
        }
    }
}
