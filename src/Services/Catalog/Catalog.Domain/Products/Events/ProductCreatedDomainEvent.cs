using Commerce.BuildingBlocks.Domain.Events;

namespace Catalog.Domain.Products.Events
{
    public sealed record ProductCreatedDomainEvent(
        Guid ProductId,
        string Name,
        decimal Price
    ) : DomainEvent;
}