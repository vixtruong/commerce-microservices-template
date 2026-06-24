using Catalog.Domain.Inventory;
using Catalog.Domain.Products;

namespace Catalog.Application.Abstractions
{
    /// <summary>
    /// Defines StockItem aggregate persistence operations.
    /// </summary>
    public interface IStockItemRepository
    {
        /// <summary>
        /// Adds a new stock item to the current unit of work.
        /// </summary>
        /// <param name="stockItem">Stock item to add.</param>
        void Add(StockItem stockItem);

        /// <summary>
        /// Gets a tracked stock item and its reservations.
        /// </summary>
        /// <param name="productId">Associated product identifier.</param>
        /// <param name="cancellationToken">
        /// Token used to cancel the database operation.
        /// </param>
        /// <returns>The stock item when found; otherwise null.</returns>
        Task<StockItem?> GetByProductIdAsync(
            ProductId productId,
            CancellationToken cancellationToken);
    }
}
