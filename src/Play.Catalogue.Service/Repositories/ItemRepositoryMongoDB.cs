using MongoDB.Driver;
using Play.Catalogue.Service.Contracts;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Repositories
{
    public class ItemRepositoryMongoDB : GenericRepositoryMongoDB<Item>, IItemRepository
    {

        private const string CollectionName = "items";

        public ItemRepositoryMongoDB(IMongoDatabase database) : base(CollectionName, database)
        {

        }

    }
}
