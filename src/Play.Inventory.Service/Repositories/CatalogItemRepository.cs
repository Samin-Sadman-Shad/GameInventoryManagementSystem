using MongoDB.Driver;
using Play.Common.MongoDb;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Repositories
{
    public class CatalogItemRepository : GenericRepositoryMongoDB<CatalogItem>
    {
        public const string CollectionName = "catalogItems";
        public CatalogItemRepository(IMongoDatabase database) : base(CollectionName, database)
        {
        }
    }
}
