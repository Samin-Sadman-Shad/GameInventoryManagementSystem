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
        IInventoryItemService _inventoryItemService;
        public InventoryController(IInventoryItemService service)
        {
            _inventoryItemService = service;
        }

        [HttpGet]
        public async Task<ActionResult<InventoryItemDto>> GetItemByUserId([FromQuery]Guid userId)
        {
            var inventoryItem = await _inventoryItemService.GetAllInventoryItems(userId);
            return Ok(inventoryItem);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> GrantInventoryItem(GrantInventoryItemDto itemDto)
        {
            var response = await _inventoryItemService.GrantInventoryItem(itemDto);
            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(response.Message);
            }
            if(response.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetItemByUserId), new {Id = response.RecordId}, response.Record);
            }
            return StatusCode(500);
        }
    }
}
