using Play.Common.Abstraction;

namespace Play.Inventory.Service.Entities
{
    public class InventoryItem : IBaseEntity
    {
        public Guid Id { get ; set ; }

        /// <summary>
        /// The Id of the user it belongs to
        /// </summary>
        public Guid UserId { get ; set ; }

        /// <summary>
        /// The Id of the Catalog item it belongs to
        /// </summary>
        public Guid CatalogId { get ; set ; }

        public int Quantity { get ; set ; }

        public DateTimeOffset DateAcquired { get ; set ; }
    }
}
