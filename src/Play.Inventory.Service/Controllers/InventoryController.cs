using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.Models;
using static Play.Inventory.Service.DTOs.Dtos;
using System.Net;

namespace Play.Inventory.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        private readonly ICatalogClient _catalogClient;
        public InventoryController(IInventoryItemService service, ICatalogClient catalogClient)
        {
            _inventoryItemService = service;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<InventoryItemDto>> GetItemByUserId([FromQuery]Guid userId)
        {
            try
            {
                var catalogItems = await _catalogClient.GetCatalogItemAsync();
                var inventoryItems = await _inventoryItemService.GetAllInventoryItems(userId);

                if(inventoryItems != null && inventoryItems.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }

                //first look into the collection of catalog items
                //find the catalog item that corresponds to the current inventory item
                var inventoryItemDtos = inventoryItems!.Records!.Select<InventoryItemDto, InventoryItemDtoExternal>(inventoryDto =>
                {
                    var catalogItem = catalogItems.Single(getCatalogDto => getCatalogDto.Id == inventoryDto.CatalogItemId);
                    return new InventoryItemDtoExternal(userId,
                        catalogItem.Name, catalogItem.Description, catalogItem.Id,
                        inventoryDto.Quantity, inventoryDto.AcquiredDate);
                });

                return Ok(inventoryItemDtos);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Exception is thrown while fetching the catalog Items and Inventory Items");
            }

        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> GrantInventoryItem(GrantInventoryItemDto itemDto)
        {
            var response = await _inventoryItemService.GrantInventoryItem(itemDto);
            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(response.Message);
            }
            else if(response.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetItemByUserId), new {Id = response.RecordId}, response.Record);
            }
            else
            {
                return StatusCode(500, response.StatusCode);
            }

        }
    }
}
