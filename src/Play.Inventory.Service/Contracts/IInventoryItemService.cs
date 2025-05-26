using Play.Inventory.Service.Models;
using static Play.Inventory.Service.DTOs.Dtos;

namespace Play.Inventory.Service.Contracts
{
    public interface IInventoryItemService
    {
        public Task<InventoryItemServiceResponse<InventoryItemDto>> GrantInventoryItem(GrantInventoryItemDto dto);
        //public Task<InventoryItemServiceResponse<InventoryItemDto>> GetInventoryItem(
        //    Guid UserId, 
        //    Guid CatalogItemId, 
        //    int Quantity, 
        //    DateTimeOffset AcquiredDate);

        //public Task<InventoryItemServiceResponse<InventoryItemDto>> GetAllInventoryItems(Guid userId);
        public Task<InventoryItemServiceResponse<InventoryItemDtoExternal>> GetAllInventoryItems(Guid userId);
    }
}
