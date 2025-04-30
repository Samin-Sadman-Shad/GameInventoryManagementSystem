using Microsoft.AspNetCore.Mvc;
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
            new GetItemDto ( Guid.NewGuid(), "Brone Sword", "Bronze Sword", 5, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Gold Coin", "Gold Coin", 4, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Armour", "Armour", 7, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Sheild", "Sheild", 9, DateTimeOffset.UtcNow),
        };

        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
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
