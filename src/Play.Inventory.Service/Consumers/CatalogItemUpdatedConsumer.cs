using MassTransit;
using Play.Catalog.Contracts.Contracts;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {

        private readonly ICatalogItemRepository _catalogItemRepository;

        public CatalogItemUpdatedConsumer(ICatalogItemRepository catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var catalogItemExisting = await _catalogItemRepository.GetAsync(message.Id);
            if (catalogItemExisting is null)
            {
                var catalogItem = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                };

                await _catalogItemRepository.AddAsync(catalogItem);
            }
            else
            {
                catalogItemExisting.Name = message.Name;
                catalogItemExisting.Description = message.Description;
                await _catalogItemRepository.UpdateAsync(catalogItemExisting);
            }


        }
    }
}
