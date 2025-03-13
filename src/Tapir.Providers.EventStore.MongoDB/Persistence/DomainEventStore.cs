using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO;
using System.Reflection;
using Tapir.Core.Domain;
using Tapir.Core.Events;
using Tapir.Core.Persistence;

namespace Tapir.Providers.EventStore.MongoDB.Persistence
{
    public class DomainEventStore : IDomainEventStore
    {
        private Configuration _configuration;
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IDomainEventRegistry _eventRegistry;

        private const string EventsCollectionName = "events";
        private const string MetaDataCollectionName = "metadata";

        private const string LastSynchronizationTimeKey = "LastSynchronizationTime";

        public DomainEventStore(Configuration configuration, IMongoClient mongoClient, IDomainEventRegistry eventRegistry)
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

        public async Task AddAsync(IEnumerable<DomainEvent> events)
        {
            using (var session = _mongoClient.StartSession())
            {
                session.StartTransaction();

                try
                {
                    await _mongoDatabase.GetCollection<DomainEvent>(EventsCollectionName).InsertManyAsync(session, events);
                    await session.CommitTransactionAsync();
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<DomainEvent>> GetByAggregateId(Guid aggregateId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("AggregateId", aggregateId);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            return documents.Select(DeserializeEvent);
        }

        public async Task<IEnumerable<DomainEvent>> GetByTimestamp(DateTime from, DateTime to)
        {
            var filter = Builders<BsonDocument>.Filter.Gt("Timestamp", from) & Builders<BsonDocument>.Filter.Lte("Timestamp", to);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            return documents.Select(DeserializeEvent);
        }

        private DomainEvent DeserializeEvent(BsonDocument document)
        {
            var type = document["_t"].AsString;
            var assemblyType = _eventRegistry.GetAssemblyType(type);
           
            return (DomainEvent)BsonSerializer.Deserialize(document, assemblyType);
        }

        public async Task<DateTime?> GetLastSynchronizationTime()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", LastSynchronizationTimeKey);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(MetaDataCollectionName).FindAsync(filter);
            var result = await documents.FirstOrDefaultAsync();

            if (result != null)
            {
                return result["Value"].AsNullableUniversalTime;
            }

            return null;
        }

        public async Task SetLastSynchronizationTime(DateTime? time)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", LastSynchronizationTimeKey);
            var update = Builders<BsonDocument>.Update.Set("Value", time);
            var options = new UpdateOptions { IsUpsert = true };

            await _mongoDatabase.GetCollection<BsonDocument>(MetaDataCollectionName).UpdateOneAsync(filter, update, options);
        }
    }
}
