using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Products
{
    public static class ProductErrors
    {
        public static readonly Error NameRequired =
            Error.Validation(
                "Catalog.Product.NameRequired",
                "Product name is required.");

        public static readonly Error PriceMustBePositive =
            Error.Validation(
                "Catalog.Product.PriceMustBePositive",
                "Product price must be greater than zero.");

        public static Error NotFound(ProductId productId)
        {
            return Error.NotFound(
                "Catalog.Product.NotFound",
                $"Product '{productId}' was not found.");
        }
    }
}
