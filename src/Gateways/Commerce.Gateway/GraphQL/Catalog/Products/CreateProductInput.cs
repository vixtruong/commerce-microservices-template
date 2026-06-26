namespace Commerce.Gateway.GraphQL.Catalog.Products
{
    /// <summary>
    /// Represents GraphQL input used to create a Catalog product.
    /// </summary>
    /// <param name="Sku">Unique product SKU.</param>
    /// <param name="Name">Product display name.</param>
    /// <param name="Description">Optional product description.</param>
    /// <param name="PriceAmount">Initial product price amount.</param>
    /// <param name="PriceCurrency">Three-character price currency code.</param>
    public sealed record CreateProductInput(
        string Sku,
        string Name,
        string? Description,
        decimal PriceAmount,
        string PriceCurrency);
}
