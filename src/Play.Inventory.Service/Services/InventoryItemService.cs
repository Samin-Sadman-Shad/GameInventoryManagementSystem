using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Models;
using Play.Common;
using Play.Inventory.Service.Mapper;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.DTOs.Dtos;
using System.Net;

namespace Play.Inventory.Service.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<InventoryItemServiceResponse<Dtos.InventoryItemDto>> GetInventoryItem(
            Guid UserId, 
            Guid CatalogItemId, 
            int Quantity, 
            DateTimeOffset AcquiredDate)
        {
            var response = new InventoryItemServiceResponse<Dtos.InventoryItemDto>();
            var itemEntities = await _inventoryItemRepository.GetInventoryItemAsync(UserId, CatalogItemId, Quantity, AcquiredDate);
            var itemDtos = itemEntities.Select<InventoryItem, InventoryItemDto>(entity => entity.AsInventoryItemDto()).ToList();
            response.StatusCode = HttpStatusCode.OK;
            response.Records = itemDtos;
            return response;
        }

        public async Task<InventoryItemServiceResponse<InventoryItemDto>> GrantInventoryItem(Dtos.GrantInventoryItemDto dto)
        {
            var response = new InventoryItemServiceResponse<InventoryItemDto>();
            try
            {
                var entityResponse = dto.AsEntity();
                if (!entityResponse.IsSuccess)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = entityResponse.Errors;
                    return response;
                }
                var inventoryItem = await _inventoryItemRepository.GetAsync(item => 
                                                                    item.UserId == dto.UserId && item.CatalogId == dto.CatalogItemId);
                if(inventoryItem is null)
                {
                    await _inventoryItemRepository.AddAsync(entityResponse.Entity!);
                }
                else
                {
                    inventoryItem.Quantity += dto.Quantity;
                    await _inventoryItemRepository.UpdateAsync(inventoryItem);
                }

                response.StatusCode = HttpStatusCode.Created;
                response.RecordId = entityResponse.Entity!.Id;
                response.Record = inventoryItem!.AsInventoryItemDto();

                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            
        }

        public async Task<InventoryItemServiceResponse<InventoryItemDto>> GetAllInventoryItems(Guid userId)
        {
            var response = new InventoryItemServiceResponse<InventoryItemDto>();

            var itemEntities = await _inventoryItemRepository.GetAllInventoryItemAsync(userId);
            var itemDtos = itemEntities.Select(entity => entity.AsInventoryItemDto()).ToList();
            response.StatusCode = HttpStatusCode.OK;
            response.Records = itemDtos;
            return response;
        }
    }
}
