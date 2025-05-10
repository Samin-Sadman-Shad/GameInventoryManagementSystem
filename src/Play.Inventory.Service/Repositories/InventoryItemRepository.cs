using MongoDB.Driver;
using Play.Common.MongoDb;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Mapper;
using Play.Inventory.Service.Models;

namespace Play.Inventory.Service.Repositories
{
    public class InventoryItemRepository : GenericRepositoryMongoDB<InventoryItem>, IInventoryItemRepository
    {
        private const string collectionName = "inventoryItems";
        public InventoryItemRepository(IMongoDatabase database) : base(collectionName, database)
        {

        }

        //public async Task<InventoryItem> CreateInventoryItemAsync(InventoryItem itemReceived)
        //{

        //    var existingItem = await base.GetAsync(item => item.UserId == itemReceived.UserId && item.CatalogId == itemReceived.CatalogId);
        //    if (existingItem is null)
        //    {
        //        await base.AddAsync(itemReceived!);
        //    }
        //    else
        //    {
        //        existingItem.Quantity += itemReceived.Quantity;
        //        await base.UpdateAsync(existingItem);
        //        itemReceived = existingItem;
        //    }
        //    return itemReceived;

        //}

        public async Task<List<InventoryItem>> GetAllInventoryItemAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return null;
            }

            var items = await base.GetAllAsync(item => item.UserId == userId);
            if (items is null)
            {
                return new List<InventoryItem>();
            }
            return items!.ToList();
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
