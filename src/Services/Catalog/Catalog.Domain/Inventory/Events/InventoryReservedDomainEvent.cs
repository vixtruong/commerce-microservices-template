using Commerce.BuildingBlocks.Domain.Events;

namespace Catalog.Domain.Inventory.Events
{
    public sealed record InventoryReservedDomainEvent(
        Guid ReservationId,
        Guid OrderId,
        Guid ProductId,
        int Quantity) : DomainEvent;
}
