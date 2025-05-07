using MongoDB.Driver;
using Play.Common.MongoDb;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Repositories
{
    public class InventoryItemRepository : GenericRepositoryMongoDB<InventoryItem>, IInventoryItemRepository
    {
        private const string collectionName = "inventoryItems";
        public InventoryItemRepository(IMongoDatabase database) : base(collectionName, database)
        {

        }

        public async Task<List<InventoryItem>> GetInventoryItemAsync(
            Guid UserId, 
            Guid CatalogItemId, 
            int Quantity, 
            DateTimeOffset AcquiredDate)
        {
            var items = await base.GetAllAsync(entity => entity.UserId == UserId
                                                        && entity.CatalogId == CatalogItemId
                                                        && entity.Quantity >= Quantity
                                                        && entity.DateAcquired == AcquiredDate);

            foreach(var item in items)
            {
                item.Quantity = item.Quantity-Quantity >= 0 ? item.Quantity-Quantity : 0;
                await base.UpdateAsync(item);
            }

            return items.ToList();
        }
    }
}
