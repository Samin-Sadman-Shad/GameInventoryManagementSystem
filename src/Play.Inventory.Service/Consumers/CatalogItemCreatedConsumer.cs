using MassTransit;
using Play.Catalog.Contracts.Contracts;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly ICatalogItemRepository _catalogItemRepository;

        public CatalogItemCreatedConsumer(ICatalogItemRepository catalogItemRepository)
        {
              _catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            try
            {
                var catalogItemExisting = await _catalogItemRepository.GetAsync(message.Id);
                if (catalogItemExisting is not null)
                {
                    return;
                }

                var catalogItem = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                };

                await _catalogItemRepository.AddAsync(catalogItem);
            }
            catch(ArgumentException ex)
            {
                var catalogItem = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                };

                await _catalogItemRepository.AddAsync(catalogItem);
            }


        }
    }
}
