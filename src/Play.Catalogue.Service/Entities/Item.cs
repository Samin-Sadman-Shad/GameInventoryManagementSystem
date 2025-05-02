using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Play.Catalogue.Service.Entities
{
    //MongoDB's driver uses BSONSerializer behind the scene to serialize the c# object to BSON to save in disk

    public class Item:IBaseEntity
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        [BsonId] 
        //treats the field as _id
        public Guid Id { get; set; }
        [BsonElement("Name")] //custom field name
        public string Name { get; set; }
        [BsonElement("Description")]
        [BsonIgnoreIfNull]
        public string Description { get; set; }
        [BsonElement("Price")]
        public decimal Price { get; set; }
        [BsonElement("DateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
