using Play.Common.Abstraction;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Models;
using static Play.Inventory.Service.DTOs.Dtos;

namespace Play.Inventory.Service.Contracts
{
    public interface IInventoryItemRepository:IGenericRepository<InventoryItem>
    {
        public Task<List<InventoryItem>> GetInventoryItemAsync(
            Guid UserId,
            Guid CatalogItemId,
            int Quantity,
            DateTimeOffset AcquiredDate);

        public Task<List<InventoryItem>> GetAllInventoryItemAsync(Guid userId);

        //public Task<InventoryItem> CreateInventoryItemAsync(InventoryItem item);
    }
}
