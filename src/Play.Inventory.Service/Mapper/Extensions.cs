using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static Play.Inventory.Service.DTOs.Dtos;

namespace Play.Inventory.Service.Mapper
{
    public static class Extensions
    {
        public static EntityResponse<InventoryItem> AsEntity(this GrantInventoryItemDto grantItemDto)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(grantItemDto, new ValidationContext(grantItemDto), validationResults);
            if (!isValid)
            {
                var errorList = validationResults.Select(x => x.ErrorMessage!).ToList();
                return EntityResponse<InventoryItem>.Failure(errorList.ToArray());
            }
            var entity = new InventoryItem
            {
                Id = Guid.NewGuid(),
                UserId = grantItemDto.UserId,
                CatalogId = grantItemDto.CatalogItemId,
                Quantity = grantItemDto.Quantity,
                DateAcquired = DateTimeOffset.UtcNow
            };

            //return entity;
            return EntityResponse<InventoryItem>.Success(entity);
        }

        public static GrantInventoryItemDto AsGrantInventoryItemDto(this InventoryItem entity)
        {
            var dto = new GrantInventoryItemDto(
                UserId: entity.UserId,
                CatalogItemId: entity.CatalogId,
                Quantity: entity.Quantity);
            return dto;
        }

        public static InventoryItemDto AsInventoryItemDto(this InventoryItem entity)
        {
            var dto = new InventoryItemDto(
                UserId: entity.UserId,
                CatalogItemId: entity.CatalogId,
                Quantity: entity.Quantity,
                AcquiredDate: entity.DateAcquired);
            return dto;
        }

        public static InventoryItemDtoExternal AsExternalDto(this InventoryItem entity, string catalogName, string catalogDescription)
        {
            var dto = new InventoryItemDtoExternal(
                UserId: entity.UserId,
                CatalogId: entity.CatalogId,
                CatalogName: catalogName,
                CatalogDescription: catalogDescription,
                Quantity: entity.Quantity,
                AcquiredDate: entity.DateAcquired);
            return dto;
        }
    }
}
