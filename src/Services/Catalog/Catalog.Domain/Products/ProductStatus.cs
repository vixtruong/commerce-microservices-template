namespace Catalog.Domain.Products
{
    /// <summary>
    /// Defines the lifecycle status of a Catalog product.
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// The product is being prepared and is not available for sale.
        /// </summary>
        Draft = 0,

        /// <summary>
        /// The product is available for sale.
        /// </summary>
        Active = 1,

        /// <summary>
        /// The product is no longer available for sale.
        /// </summary>
        Inactive = 2
    }
}
