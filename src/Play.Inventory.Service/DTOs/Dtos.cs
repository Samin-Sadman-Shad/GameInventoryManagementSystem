using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service.DTOs
{
    public class Dtos
    {
        //grant/add items to the user
        public record GrantInventoryItemDto(
            Guid UserId, 
            Guid CatalogItemId,
            [Range(0, 100)]
            int Quantity);

        //return the series of items a user possesses in the inventory
        public record RetrieveAllInventoryItemDto(Guid UserId);

        public record InventoryItemDto(Guid UserId, Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);
    }
}
