using Microsoft.AspNetCore.Mvc;
using Play.Catalogue.Service.Dtos;

namespace Play.Catalogue.Service.Controllers
{
    [ApiController]
    public class ItemsController:ControllerBase
    {
        private static readonly List<GetItemDto> Items = new List<GetItemDto>
        {
            new GetItemDto ( Guid.NewGuid(), "Brone Sword", "Bronze Sword", 5, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Gold Coin", "Gold Coin", 4, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Armour", "Armour", 7, DateTimeOffset.UtcNow),
            new GetItemDto ( Guid.NewGuid(), "Sheild", "Sheild", 9, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetItemDto>>> Get()
        {
            return Ok(Items);
        }

        [HttpGet("{id}")]
        public async Task<GetItemDto> GetById(Guid id)
        {
            var item = Items.Where(item => item.Id == id).FirstOrDefault();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<GetItemDto>> Create(CreateItemDto item)
        {
            var itemCreated = new GetItemDto(Guid.NewGuid(), item.Name, item.Description, item.price, DateTimeOffset.UtcNow);
            Items.Add(itemCreated);
            //the item has been created and you can find it at the following route
            return CreatedAtAction(nameof(GetById), new { Id = itemCreated.Id }, itemCreated);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateItemDto item)
        {
            var existingItem = await GetById(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            var updatedItem = existingItem with
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                price = item.price,
                DateCreated = DateTimeOffset.UtcNow,
            };

            //var index = Items.FindIndex(existingItem => existingItem.Id == id);
            var index = GetIndexById(id, existingItem);
            Items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingItem = await GetById(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            var index = GetIndexById(id, existingItem);
            Items.RemoveAt(index);

            return NoContent();
        }

        private int GetIndexById(Guid id, GetItemDto item)
        {
            return Items.FindIndex(item => item.Id == id);
        }
    }
}
