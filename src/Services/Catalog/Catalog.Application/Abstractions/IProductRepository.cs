using Catalog.Domain.Products;

namespace Catalog.Application.Abstractions
{
    /// <summary>
    /// Defines Product aggregate persistence operations.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Adds a new Product aggregate to the current unit of work.
        /// </summary>
        /// <param name="product">Product aggregate to add.</param>
        void Add(Product product);

        /// <summary>
        /// Gets a product for read-only access.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="cancellationToken">
        /// Token used to cancel the database operation.
        /// </param>
        /// <returns>The product when found; otherwise null.</returns>
        Task<Product?> GetByIdAsync(
            ProductId productId,
            CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether a normalized SKU already exists.
        /// </summary>
        /// <param name="sku">Normalized product SKU.</param>
        /// <param name="cancellationToken">
        /// Token used to cancel the database operation.
        /// </param>
        /// <returns>True when the SKU exists.</returns>
        Task<bool> ExistsBySkuAsync(
            string sku,
            CancellationToken cancellationToken);

        Task<bool> ExistsByIdAsync(
            ProductId productId,
            CancellationToken cancellationToken);
    }
}
