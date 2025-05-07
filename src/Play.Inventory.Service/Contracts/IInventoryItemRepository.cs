using Play.Common.Abstraction;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Contracts
{
    public interface IInventoryItemRepository:IGenericRepository<InventoryItem>
    {
        public Task<List<InventoryItem>> GetInventoryItemAsync(
            Guid UserId,
            Guid CatalogItemId,
            int Quantity,
            DateTimeOffset AcquiredDate);
    }
}
