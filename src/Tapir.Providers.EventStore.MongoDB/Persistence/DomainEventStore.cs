using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Tapir.Core.Domain;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Providers.EventStore.MongoDB.Documents;

namespace Tapir.Providers.EventStore.MongoDB.Persistence
{
    public class DomainEventStore : IDomainEventStore
    {
        private Configuration _configuration;
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IDomainEventRegistry _eventRegistry;

        private const string AggregatesCollectionName = "aggregates";
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

        public async Task<bool> AddAsync(Guid aggregateId, DomainEvent @event, int expectedVersion)
        {
            if (!await SetAggregateVersion(aggregateId, 1, expectedVersion))
            {
                return false;
            }

            await _mongoDatabase.GetCollection<DomainEvent>(EventsCollectionName).InsertOneAsync(@event);
            return true;
        }

        public async Task<bool> AddAsync(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion)
        {
            if (!await SetAggregateVersion(aggregateId, events.Count(), expectedVersion))
            {
                return false;
            }

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

            return true;
        }

        public async Task<IReadOnlyList<DomainEvent>> GetByAggregateId(Guid aggregateId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("AggregateId", aggregateId);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            return documents.Select(DeserializeEvent).ToList();
        }

        public async Task<IReadOnlyList<DomainEvent>> GetByTimestamp(DateTime from, DateTime to)
        {
            var filter = Builders<BsonDocument>.Filter.Gt("Timestamp", from) & Builders<BsonDocument>.Filter.Lte("Timestamp", to);
            var documents = await _mongoDatabase.GetCollection<BsonDocument>(EventsCollectionName).Find(filter).ToListAsync();

            return documents.Select(DeserializeEvent).ToList();
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

        private async Task<bool> SetAggregateVersion(Guid aggregateId, int eventsCount, int expectedVersion)
        {
            var collection = _mongoDatabase.GetCollection<AggregateDocument>(AggregatesCollectionName);
            var filter = Builders<AggregateDocument>.Filter.Eq("_id", aggregateId);

            if (await collection.CountDocumentsAsync(filter) == 0)
            {
                await collection.InsertOneAsync(new AggregateDocument
                {
                    Id = aggregateId,
                    Version = eventsCount
                });

                return true;
            }
            else
            {
                filter &= Builders<AggregateDocument>.Filter.Eq("Version", expectedVersion);

                var update = Builders<AggregateDocument>.Update.Set("Version", expectedVersion + eventsCount);
                var result = await _mongoDatabase.GetCollection<AggregateDocument>(AggregatesCollectionName).FindOneAndUpdateAsync(filter, update);

                return result != null;
            }
        }

        private DomainEvent DeserializeEvent(BsonDocument document)
        {
            var type = document["_t"].AsString;
            var assemblyType = _eventRegistry.GetAssemblyType(type);

            return (DomainEvent)BsonSerializer.Deserialize(document, assemblyType);
        }
    }
}
