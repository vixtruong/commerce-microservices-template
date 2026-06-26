using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Products
{
    /// <summary>
    /// Provides centralized product error definitions for the Catalog domain.
    /// </summary>
    public static class ProductErrors
    {
        /// <summary>
        /// Error returned when a product identifier is required but was not supplied.
        /// </summary>
        public static readonly Error IdRequired =
            Error.Validation(
                "Catalog.Product.IdRequired",
                "Product id is required.");

        /// <summary>
        /// Error returned when a product identifier cannot be parsed from the incoming contract.
        /// </summary>
        public static readonly Error IdInvalid =
            Error.Validation(
                "Catalog.Product.IdInvalid",
                "Product id is invalid.");

        /// <summary>
        /// Error returned when a product SKU is required but was not supplied.
        /// </summary>
        public static readonly Error SkuRequired =
            Error.Validation(
                "Catalog.Product.SkuRequired",
                "Product SKU is required.");

        /// <summary>
        /// Error returned when a product name is required but was not supplied.
        /// </summary>
        public static readonly Error NameRequired =
            Error.Validation(
                "Catalog.Product.NameRequired",
                "Product name is required.");

        /// <summary>
        /// Error returned when a product price must be positive.
        /// </summary>
        public static readonly Error PriceMustBePositive =
            Error.Validation(
                "Catalog.Product.PriceMustBePositive",
                "Product price must be greater than zero.");

        /// <summary>
        /// Error returned when the product price amount cannot be parsed from the incoming contract.
        /// </summary>
        public static readonly Error PriceAmountInvalid =
            Error.Validation(
                "Catalog.Product.PriceAmountInvalid",
                "Price amount is invalid.");

        /// <summary>
        /// Creates an error returned when a product SKU is already used by another product.
        /// </summary>
        /// <param name="sku">Normalized SKU that already exists.</param>
        /// <returns>The duplicate SKU error.</returns>
        public static Error SkuAlreadyExists(string sku)
        {
            return Error.Conflict(
                "Catalog.Product.SkuAlreadyExists",
                $"Product SKU '{sku}' already exists.");
        }

        /// <summary>
        /// Creates an error returned when a product cannot be found.
        /// </summary>
        /// <param name="productId">Product identifier that was requested.</param>
        /// <returns>The product not-found error.</returns>
        public static Error NotFound(ProductId productId)
        {
            return Error.NotFound(
                "Catalog.Product.NotFound",
                $"Product '{productId}' was not found.");
        }
    }
}
