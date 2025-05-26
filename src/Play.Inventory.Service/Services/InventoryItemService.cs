using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Models;
using Play.Common;
using Play.Inventory.Service.Mapper;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.DTOs.Dtos;
using System.Net;
using System.Collections.Immutable;
using MassTransit;

namespace Play.Inventory.Service.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ICatalogItemRepository _catalogItemRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository, ICatalogItemRepository catalogItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _catalogItemRepository = catalogItemRepository;
        }

        //public async Task<InventoryItemServiceResponse<Dtos.InventoryItemDto>> GetInventoryItem(
        //    Guid UserId, 
        //    Guid CatalogItemId, 
        //    int Quantity, 
        //    DateTimeOffset AcquiredDate)
        //{
        //    var response = new InventoryItemServiceResponse<Dtos.InventoryItemDto>();
        //    var itemEntities = await _inventoryItemRepository.GetInventoryItemAsync(UserId, CatalogItemId, Quantity, AcquiredDate);
        //    var itemDtos = itemEntities.Select<InventoryItem, InventoryItemDto>(entity => entity.AsInventoryItemDto()).ToList();
        //    response.StatusCode = HttpStatusCode.OK;
        //    response.Records = itemDtos;
        //    return response;
        //}

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
                    response.Record = entityResponse.Entity!.AsInventoryItemDto();
                }
                else
                {
                    inventoryItem.Quantity += dto.Quantity;
                    await _inventoryItemRepository.UpdateAsync(inventoryItem);
                    response.Record = inventoryItem!.AsInventoryItemDto();
                }

                response.StatusCode = HttpStatusCode.Created;
                response.RecordId = entityResponse.Entity!.Id;
                

                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            
        }

        public async Task<InventoryItemServiceResponse<InventoryItemDtoExternal>> GetAllInventoryItems(Guid userId)
        {
            //var response = new InventoryItemServiceResponse<InventoryItemDto>();
            var response = new InventoryItemServiceResponse<InventoryItemDtoExternal>();
            try
            {
                if (userId == Guid.Empty)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var itemEntities = await _inventoryItemRepository.GetAllInventoryItemAsync(userId);

                var catalogItemIds = itemEntities.Select(x => x.CatalogId);
                var catalogItemEntities = await _catalogItemRepository.GetAllAsync(catalogItem => catalogItemIds.Contains(catalogItem.Id));

                //var itemDtos = itemEntities.Select(entity => entity.AsInventoryItemDto()).ToList();
                var itemDtos = itemEntities.Select(inventoryItem =>
                {
                    var catalogItem = catalogItemEntities.Single(item => item.Id == inventoryItem.CatalogId);
                    return inventoryItem.AsExternalDto(catalogItem.Name, catalogItem.Description);
                });
                response.StatusCode = HttpStatusCode.OK;
                response.Records = itemDtos.ToList();
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

        }

        public async Task<InventoryItemServiceResponse<Guid>> GetUserIds()
        {
            var response = new InventoryItemServiceResponse<Guid>();
            try
            {
                var inventoryItems = await _inventoryItemRepository.GetAllAsync();
                if (inventoryItems == null)
                {
                    response.Records = null;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                var userIds = inventoryItems!.Select(x => x.UserId)
                                             .Distinct()
                                             .ToImmutableList();
                response.Records = userIds;
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }


        }
    }
}
