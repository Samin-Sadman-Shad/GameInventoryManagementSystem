using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Entities;
using Play.Catalogue.Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalogue.Service.Mapping
{
    public static class Extensions
    {
        public static GetItemDto AsGetDto(this Item item)
        {
            return new GetItemDto(item.Id, item.Name, item.Description, item.Price, item.DateCreated);
        }

        public static Item AsItem(this GetItemDto dto)
        {
            return new Item 
            { 
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.price,
                DateCreated = dto.DateCreated
            };
        }

        public static Item AsEntity(this CreateItemDto itemDto)
        {
            try
            {
                var item = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Description = itemDto.Description,
                    Price = itemDto.price,
                    DateCreated = DateTimeOffset.UtcNow
                };
                return item;
            }
            catch(Exception ex)
            {
                throw new BadHttpRequestException("The request body can not be converted to the Entity");
            }

        }

        public static EntityResult<Item> AsItem(this CreateItemDto itemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.price,
                DateCreated = DateTimeOffset.UtcNow
            };

            var context = new ValidationContext(item);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(item, context, validationResults, true);
            if (!isValid)
            {
                var errors = validationResults.Select(x => x.ErrorMessage!).ToList();
                return EntityResult<Item>.Failure(errors.ToArray());
            }
            return EntityResult<Item>.Success(item);
        }

        public static EntityResult<UpdateItemDto> CheckValidityForUpdate(this UpdateItemDto itemDto)
        {
            var context = new ValidationContext(itemDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(itemDto, context, validationResults, true);
            if (!isValid)
            {
                var errors = validationResults.Select(x => x.ErrorMessage!).ToList();
                return EntityResult<UpdateItemDto>.Failure(errors.ToArray());
            }
            return EntityResult<UpdateItemDto>.Success(itemDto);
        }
    }
}
