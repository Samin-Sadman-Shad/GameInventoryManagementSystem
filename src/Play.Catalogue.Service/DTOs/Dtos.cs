using System;

namespace Play.Catalogue.Service.Dtos
{
    public record GetItemDto(Guid Id, string Name, string Description, decimal price, DateTimeOffset DateCreated);
    public record CreateItemDto(string Name, string Description, decimal price);
    public record UpdateItemDto(string Name, string Description, decimal price);
    public record DeactivateItemDto(Guid Id);
    public record DeactivateItemDtoWithName(string Name);
    public record ActivateItemDtoWithName(string Name);
    public record ActivateItemDto(Guid Id);
    public record DeleteItemDto(Guid Id);
}