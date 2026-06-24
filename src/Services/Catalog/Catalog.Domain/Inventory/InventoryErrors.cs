using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Inventory
{
    public static class InventoryErrors
    {
        public static readonly Error QuantityMustBePositive = Error.Validation(
            "Catalog.Inventory.QuantityMustBePositive",
            "Quantity must be greater than zero.");

        public static readonly Error InsufficientStock = Error.Conflict(
            "Catalog.Inventory.InsufficientStock",
            "There is not enough available stock.");

        public static readonly Error ReservationAlreadyExists = Error.Conflict(
            "Catalog.Inventory.ReservationAlreadyExists",
            "An active reservation already exists for this order.");

        public static readonly Error ReservationNotFound = Error.NotFound(
            "Catalog.Inventory.ReservationNotFound",
            "The stock reservation was not found.");

        public static readonly Error ReservationNotPending = Error.Conflict(
            "Catalog.Inventory.ReservationNotPending",
            "Only a pending reservation can be confirmed or released.");

        public static readonly Error OrderIdRequired = Error.Validation(
            "Catalog.Inventory.OrderIdRequired",
            "Order id is required.");
    }
}
