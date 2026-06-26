namespace Commerce.Gateway.GraphQL.Catalog.Products
{
    /// <summary>
    /// Represents Catalog product data returned through the GraphQL gateway.
    /// </summary>
    /// <param name="Id">Product identifier.</param>
    /// <param name="Sku">Unique product SKU.</param>
    /// <param name="Name">Product display name.</param>
    /// <param name="Description">Product description.</param>
    /// <param name="PriceAmount">Product price amount.</param>
    /// <param name="PriceCurrency">Product price currency.</param>
    /// <param name="Status">Product lifecycle status.</param>
    public sealed record ProductDto(
        Guid Id,
        string Sku,
        string Name,
        string Description,
        decimal PriceAmount,
        string PriceCurrency,
        string Status);
}
