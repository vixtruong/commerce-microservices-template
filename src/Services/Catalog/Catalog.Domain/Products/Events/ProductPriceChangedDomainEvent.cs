using Commerce.BuildingBlocks.Domain.Events;

namespace Catalog.Domain.Products.Events
{
    public sealed record ProductPriceChangedDomainEvent(
        Guid ProductId,
        decimal PreviousPrice,
        decimal CurrentAmount,
        string Currency
    ) : DomainEvent;
}
