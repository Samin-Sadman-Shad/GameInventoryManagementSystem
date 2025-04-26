using System;

namespace Play.Catalogue.Service.Dtos
{
    public record GetItemDto(Guid Id, string Name, string Description, decimal price, DateTime DateCreated);
    public record CreateItemDto(string Name, string Description, decimal price);
    public record UpdateItemDto(string Name, string Description, decimal price);
    public record DeactiveItemDto(Guid Id);
    public record DeactivateItemDto(string Name);
    public record ActivateItemDto(string Name);
    public record ActivateItemDto(Guid Id);
    public record DeleteItemDto(Guid Id);
}