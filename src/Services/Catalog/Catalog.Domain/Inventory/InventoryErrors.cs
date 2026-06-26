using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Inventory
{
    /// <summary>
    /// Provides centralized inventory error definitions for the Catalog domain.
    /// </summary>
    public static class InventoryErrors
    {
        /// <summary>
        /// Error returned when a product identifier is required for a stock item operation.
        /// </summary>
        public static readonly Error ProductIdRequired = Error.Validation(
            "Catalog.StockItem.ProductIdRequired",
            "Product id is required.");

        /// <summary>
        /// Error returned when a product identifier cannot be parsed from the incoming inventory contract.
        /// </summary>
        public static readonly Error ProductIdInvalid = Error.Validation(
            "Catalog.StockItem.ProductIdInvalid",
            "Product id is invalid.");

        /// <summary>
        /// Error returned when inventory quantity must be positive.
        /// </summary>
        public static readonly Error QuantityMustBePositive = Error.Validation(
            "Catalog.Inventory.QuantityMustBePositive",
            "Quantity must be greater than zero.");

        /// <summary>
        /// Error returned when available stock is lower than the requested reservation quantity.
        /// </summary>
        public static readonly Error InsufficientStock = Error.Conflict(
            "Catalog.Inventory.InsufficientStock",
            "There is not enough available stock.");

        /// <summary>
        /// Error returned when an active reservation already exists for the same order.
        /// </summary>
        public static readonly Error ReservationAlreadyExists = Error.Conflict(
            "Catalog.Inventory.ReservationAlreadyExists",
            "An active reservation already exists for this order.");

        /// <summary>
        /// Error returned when a stock reservation cannot be found.
        /// </summary>
        public static readonly Error ReservationNotFound = Error.NotFound(
            "Catalog.Inventory.ReservationNotFound",
            "The stock reservation was not found.");

        /// <summary>
        /// Error returned when a reservation operation requires a pending reservation.
        /// </summary>
        public static readonly Error ReservationNotPending = Error.Conflict(
            "Catalog.Inventory.ReservationNotPending",
            "Only a pending reservation can be confirmed or released.");

        /// <summary>
        /// Error returned when an order identifier is required for a reservation operation.
        /// </summary>
        public static readonly Error OrderIdRequired = Error.Validation(
            "Catalog.Inventory.OrderIdRequired",
            "Order id is required.");

        /// <summary>
        /// Creates an error returned when a stock item cannot be found for a product.
        /// </summary>
        /// <param name="productId">Product identifier whose stock item was requested.</param>
        /// <returns>The stock item not-found error.</returns>
        public static Error StockItemNotFound(Guid productId)
        {
            return Error.NotFound(
                "Catalog.StockItem.NotFound",
                $"Stock item for product '{productId}' was not found.");
        }

        /// <summary>
        /// Creates an error returned when a stock item already exists for a product.
        /// </summary>
        /// <param name="productId">Product identifier whose stock item already exists.</param>
        /// <returns>The duplicate stock item error.</returns>
        public static Error StockItemAlreadyExists(Guid productId)
        {
            return Error.Conflict(
                "Catalog.StockItem.AlreadyExists",
                $"Stock item for product '{productId}' already exists.");
        }
    }
}
