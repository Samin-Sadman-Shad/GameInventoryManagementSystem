namespace Play.Catalogue.Service.Entities
{
    public class Item:IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public bool IsActive { get; set; }
    }
}
