using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts.Contracts;
using Play.Catalogue.Service.Contracts;
using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController:ControllerBase
    {
        private static readonly List<GetItemDto> Items = new List<GetItemDto>
        {
            new GetItemDto ( Guid.NewGuid(), "Bronze Sword", "Bronze Sword", 5, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Gold Coin", "Gold Coin", 4, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Armour", "Armour", 7, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Sheild", "Sheild", 9, DateTimeOffset.UtcNow),
        };

        private readonly IItemService _itemService;
        //Allow to communicate that we want to send message to some location
        //publish a message and send that message
        private readonly IPublishEndpoint _publishEndpoint;
        public ItemsController(IItemService itemService, IPublishEndpoint publishEndpoint)
        {
            _itemService = itemService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetItemDto>>> Get()
        {
            var response = await _itemService.GetAllItemsAsync();
            if (response.IsSuccess)
            {
                return Ok(response.Records);
            }
            else
            {
                return StatusCode(500);
            }
            //return Ok(Items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetItemDto>> GetById(Guid id)
        {
            //var item = Items.Where(item => item.Id == id).FirstOrDefault();
            //if(item is null)
            //{
            //    return NotFound();
            //}
            //return Ok(item);
            var response = await _itemService.GetItemByIdAsync(id);
            if (!response.IsSuccess)
            {
                return StatusCode(500);
            }
            if(response.RecordId is null)
            {
                return NotFound();
            }
            return Ok(response.Record);

        }

        [HttpPost]
        public async Task<ActionResult<GetItemDto>> Create(CreateItemDto item)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            //var itemCreated = new GetItemDto(Guid.NewGuid(), item.Name, item.Description, item.price, DateTimeOffset.UtcNow);
            //Items.Add(itemCreated);
            ////the item has been created and you can find it at the following route
            //return CreatedAtAction(nameof(GetById), new { Id = itemCreated.Id }, itemCreated);
            var response = await _itemService.CreateItemAsync(item);
            //just after creating the item in our database,
            //publish a message announcing that item has been created
            await _publishEndpoint.Publish(
                new CatalogItemCreated(response.Record.Id, response.Record.Name, response.Record.Description));
            if (!response.IsSuccess)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(response.Errors);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            return CreatedAtAction(nameof(GetById), new { Id = response.RecordId }, response.Record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateItemDto item)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            //var existingItem = Items.Where(item => item.Id == id).FirstOrDefault();
            //if(existingItem is null)
            //{
            //    return NotFound();
            //}
            //var updatedItem = existingItem with
            //{
            //    Id = id,
            //    Name = item.Name,
            //    Description = item.Description,
            //    price = item.price,
            //    DateCreated = DateTimeOffset.UtcNow,
            //};

            //var index = GetIndexById(id, existingItem);
            //Items[index] = updatedItem;

            //return NoContent();

            var response = await _itemService.UpdateItemAsync(id, item);

            await _publishEndpoint.Publish( new CatalogItemUpdated(id, item.Name, item.Description));
            if (!response.IsSuccess)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(response.Errors);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var existingItem = Items.Where(item => item.Id == id).FirstOrDefault();
            //if (existingItem is null)
            //{
            //    return NotFound();
            //}
            //var index = GetIndexById(id, existingItem);
            //Items.RemoveAt(index);

            //return NoContent();
            var response = await _itemService.DeleteItemAsync(id);

            await _publishEndpoint.Publish(new CatalogItemDeleted(id));
            if (!response.IsSuccess)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }
            return NoContent();
        }

        private int GetIndexById(Guid id, GetItemDto item)
        {
            return Items.FindIndex(item => item.Id == id);
        }
    }
}
