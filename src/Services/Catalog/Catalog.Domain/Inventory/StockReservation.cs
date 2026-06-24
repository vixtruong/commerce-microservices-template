using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Domain.Entities;
using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Inventory
{
    public class StockReservation : Entity<StockReservationId>
    {
        /// <summary>
        /// Initializes an empty reservation for Entity Framework Core materialization.
        /// </summary>
        private StockReservation()
        {
        }

        internal StockReservation(
            StockReservationId id,
            ProductId productId,
            Guid orderId,
            int quantity,
            DateTimeOffset createdAtUtc,
            DateTimeOffset expiresAtUtc)
            : base(id)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Status = StockReservationStatus.Pending;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }

        public ProductId ProductId { get; private set; }

        public Guid OrderId { get; private set; }

        public int Quantity { get; private set; }

        public StockReservationStatus Status { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset ExpiresAtUtc { get; private set; }

        public DateTimeOffset? CompletedAtUtc { get; private set; }

        public string? CompletionReason { get; private set; }

        internal Result Confirm(DateTimeOffset completedAtUtc)
        {
            if (Status != StockReservationStatus.Pending)
            {
                return InventoryErrors.ReservationNotPending;
            }

            Status = StockReservationStatus.Confirmed;
            CompletedAtUtc = completedAtUtc;
            CompletionReason = "confirmed";
            return Result.Success();
        }

        internal Result Release(string reason, DateTimeOffset completedAtUtc)
        {
            if (Status != StockReservationStatus.Pending)
            {
                return InventoryErrors.ReservationNotPending;
            }

            Status = StockReservationStatus.Released;
            CompletedAtUtc = completedAtUtc;
            CompletionReason = reason;
            return Result.Success();
        }

        internal Result Expire(DateTimeOffset completedAtUtc)
        {
            if (Status != StockReservationStatus.Pending)
            {
                return InventoryErrors.ReservationNotPending;
            }

            Status = StockReservationStatus.Expired;
            CompletedAtUtc = completedAtUtc;
            CompletionReason = "expired";
            return Result.Success();
        }
    }
}
