using MongoDB.Bson.Serialization.Attributes;
using Play.Common.Abstraction;

namespace Play.Inventory.Service.Entities
{
    public class CatalogItem : IBaseEntity
    {
        public Guid Id { get ; set ; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
