namespace Play.Inventory.Service.DTOs
{
    public class Dtos
    {
        //grant/add items to the user
        public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);

        //return the series of items a user possesses in the inventory
        public record RetrieveAllInventoryItemDto(Guid UserId);

        public record RetrieveInventoryItemByItemIdDto(Guid UserId, Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);
    }
}
