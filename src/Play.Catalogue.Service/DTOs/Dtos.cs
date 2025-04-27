using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalogue.Service.Dtos
{
    public record GetItemDto(Guid Id, string Name, string Description, decimal price, DateTimeOffset DateCreated);
    public record CreateItemDto(
        [Required]
        [MaxLength(10)]
        string Name, 
        string Description,
        [Range(0, 100)]
        decimal price);
    public record UpdateItemDto([Required]
        [MaxLength(10)]
        string Name,
        string Description,
        [Range(0, 100)]
        decimal price);
    public record DeactivateItemDto(Guid Id);
    public record DeactivateItemDtoWithName(string Name);
    public record ActivateItemDtoWithName(string Name);
    public record ActivateItemDto(Guid Id);
    public record DeleteItemDto(Guid Id);
}