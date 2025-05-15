using static Play.Inventory.Service.DTOs.Dtos;

namespace Play.Inventory.Service.Contracts
{
    public interface ICatalogClient
    {

        Task<IReadOnlyCollection<GetCatalogItemDto>> GetCatalogItemAsync();
    }
}
