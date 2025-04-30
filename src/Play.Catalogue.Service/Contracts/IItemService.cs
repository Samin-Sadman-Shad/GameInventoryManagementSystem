using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Models;

namespace Play.Catalogue.Service.Contracts
{
    public interface IItemService
    {
        public Task<ItemServiceResponse<GetItemDto>> GetAllItemsAsync();
        public Task<ItemServiceResponse<GetItemDto>> GetItemByIdAsync(Guid id);
        public Task<ItemServiceResponse<GetItemDto>> CreateItemAsync(CreateItemDto itemDto);
        public Task<ItemServiceResponse> UpdateItemAsync(Guid id, UpdateItemDto itemDto);
        public Task<ItemServiceResponse> DeleteItemAsync(Guid id);
    }
}
