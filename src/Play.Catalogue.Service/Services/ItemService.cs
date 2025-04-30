using Play.Catalogue.Service.Contracts;
using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Mapping;
using Play.Catalogue.Service.Models;
using System.Net;

namespace Play.Catalogue.Service.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemServiceResponse<GetItemDto>> GetAllItemsAsync()
        {
            var response = new ItemServiceResponse<GetItemDto>();
            try
            {
                var items = (await _itemRepository.GetAllAsync()).Select(item => item.AsGetDto());
                if (items is null)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Records = items.ToList();
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.ExpectationFailed;
            }

            return response;

        }

        public async Task<ItemServiceResponse<GetItemDto>> GetItemByIdAsync(Guid id)
        {
            var response = new ItemServiceResponse<GetItemDto>();
            try
            {
                var item = await _itemRepository.GetAsync(id);
                if (item is null)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.RecordId = item.Id;
                response.Record = item.AsGetDto();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.ExpectationFailed;
            }

            return response;
        }

        public async Task<ItemServiceResponse<GetItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            var response = new ItemServiceResponse<GetItemDto>();
            try
            {
                var entityResult = itemDto.AsItem();
                if (entityResult.IsSuccess)
                {
                    //var item = itemDto.AsEntity();
                    var item = entityResult.Entity;
                    var itemCreated = await _itemRepository.AddAsync(item);
                    var itemDtoGenerated = itemCreated.AsGetDto();
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.Created;
                    response.RecordId = itemDtoGenerated.Id;
                    response.Record = itemDtoGenerated;
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = entityResult.Errors;
                }

            }
            catch(BadHttpRequestException badRequest)
            {
                response.IsSuccess = false;
                response.StatusCode =HttpStatusCode.BadRequest;

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return response;

        }

        public async Task<ItemServiceResponse> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var response = new ItemServiceResponse();
            try
            {
                var checkItemExist = await GetItemByIdAsync(id);
                if (!checkItemExist.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                    return response;
                }
                if (checkItemExist.RecordId is null)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                var checkValidEntity = itemDto.CheckValidityForUpdate();
                if (!checkValidEntity.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var existingItem = checkItemExist.Record.AsItem();
                existingItem.Name = itemDto.Name;
                existingItem.Description = itemDto.Description;
                existingItem.Price = itemDto.price;

                await _itemRepository.UpdateAsync(existingItem);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return response;

        }

        public async Task<ItemServiceResponse> DeleteItemAsync(Guid id)
        {
            var response = new ItemServiceResponse();
            try
            {
                var checkItemExist = await GetItemByIdAsync(id);
                if (!checkItemExist.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                    return response;
                }
                if (checkItemExist.RecordId is null)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                await _itemRepository.DeleteAsync(checkItemExist.RecordId.Value);
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return response;
        }
    }
}
