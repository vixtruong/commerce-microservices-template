using Catalog.Domain.Inventory.Events;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Domain.Entities;
using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Inventory
{
    public class StockItem : AggregateRoot<ProductId>
    {
        private readonly List<StockReservation> _reservations = [];

        private StockItem(ProductId productId, DateTimeOffset createdAtUtc)
        : base(productId)
        {
            QuantityOnHand = 0;
            ReservedQuantity = 0;
            CreatedAtUtc = createdAtUtc;
            UpdatedAtUtc = createdAtUtc;
        }

        /// <summary>
        /// Initializes an empty stock item for Entity Framework Core materialization.
        /// </summary>
        private StockItem()
        {
        }

        public int QuantityOnHand { get; private set; }

        public int ReservedQuantity { get; private set; }

        public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset UpdatedAtUtc { get; private set; }

        public IReadOnlyCollection<StockReservation> Reservations =>
            _reservations.AsReadOnly();

        public static StockItem Create(ProductId productId, DateTimeOffset createdAtUtc)
        {
            return new StockItem(productId, createdAtUtc);
        }

        public Result IncreaseStock(int quantity, DateTimeOffset changedAtUtc)
        {
            if (quantity <= 0)
            {
                return InventoryErrors.QuantityMustBePositive;
            }

            QuantityOnHand += checked(QuantityOnHand + quantity);
            UpdatedAtUtc = changedAtUtc;
            return Result.Success();
        }

        public Result<StockReservationId> Reserve(
            Guid orderId,
            int quantity,
            DateTimeOffset expiresAtUtc,
            DateTimeOffset createdAtUtc)
        {
            if (orderId == Guid.Empty)
            {
                return InventoryErrors.OrderIdRequired;
            }

            if (quantity <= 0)
            {
                return InventoryErrors.QuantityMustBePositive;
            }

            if (AvailableQuantity < quantity)
            {
                return InventoryErrors.InsufficientStock;
            }

            bool hasActiveReservation = _reservations.Any(r => r.OrderId == orderId && r.Status == StockReservationStatus.Pending);

            if (hasActiveReservation)
            {
                return InventoryErrors.ReservationAlreadyExists;
            }

            var reservation = new StockReservation(
                StockReservationId.New(),
                Id,
                orderId,
                quantity,
                createdAtUtc,
                expiresAtUtc);

            _reservations.Add(reservation);
            ReservedQuantity = checked(ReservedQuantity + quantity);
            UpdatedAtUtc = createdAtUtc;

            RaiseDomainEvent(new Events.InventoryReservedDomainEvent(
                reservation.Id.Value,
                orderId,
                Id.Value,
                quantity));

            return reservation.Id;
        }

        public Result ConfirmReservation(
            StockReservationId reservationId,
            DateTimeOffset completedAtUtc)
        {
            StockReservation? reservation = FindReservation(reservationId);

            if (reservation is null)
            {
                return InventoryErrors.ReservationNotFound;
            }

            Result result = reservation.Confirm(completedAtUtc);

            if (result.IsFailure)
            {
                return result;
            }

            ReservedQuantity -= reservation.Quantity;
            QuantityOnHand -= reservation.Quantity;
            UpdatedAtUtc = completedAtUtc;
            return Result.Success();
        }

        public Result ReleaseReservation(
            StockReservationId reservationId,
            string reason,
            DateTimeOffset completedAtUtc)
        {
            StockReservation? reservation = FindReservation(reservationId);

            if (reservation is null)
            {
                return InventoryErrors.ReservationNotFound;
            }

            Result result = reservation.Release(reason, completedAtUtc);

            if (result.IsFailure)
            {
                return result;
            }

            ReservedQuantity -= reservation.Quantity;
            UpdatedAtUtc = completedAtUtc;

            RaiseDomainEvent(
                new InventoryReservationReleasedDomainEvent(
                    reservation.Id.Value,
                    reservation.OrderId,
                    Id.Value,
                    reservation.Quantity,
                    reason));

            return Result.Success();
        }

        public Result ExpireReservation(
            StockReservationId reservationId,
            DateTimeOffset completedAtUtc)
        {
            StockReservation? reservation = FindReservation(reservationId);

            if (reservation is null)
            {
                return InventoryErrors.ReservationNotFound;
            }

            Result result = reservation.Expire(completedAtUtc);

            if (result.IsFailure)
            {
                return result;
            }

            ReservedQuantity -= reservation.Quantity;
            UpdatedAtUtc = completedAtUtc;
            return Result.Success();
        }

        private StockReservation? FindReservation(StockReservationId reservationId) =>
            _reservations.SingleOrDefault(reservation => reservation.Id == reservationId);
    }
}
