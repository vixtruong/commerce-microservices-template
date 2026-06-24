using Commerce.BuildingBlocks.Domain.Events;

namespace Catalog.Domain.Inventory.Events
{
    public sealed record InventoryReservationReleasedDomainEvent(
    Guid ReservationId,
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    string Reason) : DomainEvent;
}
