namespace Catalog.Domain.Inventory
{
    /// <summary>
    /// Defines the lifecycle status of an inventory reservation.
    /// </summary>
    public enum StockReservationStatus
    {
        /// <summary>
        /// The reservation is awaiting confirmation or release.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The reservation has been confirmed for the order.
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// The reserved inventory has been returned to availability.
        /// </summary>
        Released = 2,

        /// <summary>
        /// The reservation elapsed before it was completed.
        /// </summary>
        Expired = 3
    }
}
