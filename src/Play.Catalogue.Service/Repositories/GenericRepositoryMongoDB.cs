using MongoDB.Driver;
using Play.Catalogue.Service.Contracts;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Repositories
{
    public class GenericRepositoryMongoDB<T> : IGenericRepository<T> where T:IBaseEntity
    {
        protected readonly string _collectionName;
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<T> _dbCollection;
        protected readonly FilterDefinitionBuilder<T> filterBuilder;
        protected readonly ILogger _logger;

        public GenericRepositoryMongoDB(string collectionName, IMongoDatabase database)
        {
            _collectionName = collectionName;
            //var mongoClient = new MongoClient("mongodb://localhost:27017");
            //_database = mongoClient.GetDatabase("Catalog");
            _database = database;
            _dbCollection = _database.GetCollection<T>(_collectionName);
            filterBuilder = Builders<T>.Filter;

        }

        public async Task<T> AddAsync(T entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
             await _dbCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var filter = filterBuilder.Eq(t => t.Id, id);
            var cursor = await _dbCollection.FindAsync(filter);
            var entity = await cursor.SingleOrDefaultAsync();
            if(entity is null)
            {
                throw new ArgumentException("Item not found with this id", nameof(entity.Id));
            }
             var deleteResult = await _dbCollection.DeleteOneAsync(filter);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var cursor = await _dbCollection.FindAsync(FilterDefinition<T>.Empty);
            var entities = await cursor.ToListAsync();
            return entities;
        }

        public async Task<T> GetAsync(Guid id)
        {
            var entity = await GetSingleEntityById(id);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            var replaceResult = await _dbCollection.ReplaceOneAsync(filter, entity);
            return entity;
        }

        protected async Task<T> GetSingleEntityById(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(t => t.Id, id);
            var cursor = await _dbCollection.FindAsync(filter);
            var entity = await cursor.FirstOrDefaultAsync();
            if (entity is null)
            {
                throw new ArgumentException("Item not found with this id", nameof(entity.Id));
            }
            return entity;
        }
    }
}
