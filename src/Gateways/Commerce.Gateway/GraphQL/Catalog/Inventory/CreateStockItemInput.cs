namespace Commerce.Gateway.GraphQL.Catalog.Inventory
{
    /// <summary>
    /// Represents GraphQL input used to create a stock item for a Catalog product.
    /// </summary>
    /// <param name="ProductId">Identifier of the product that should receive a stock item.</param>
    public sealed record CreateStockItemInput(Guid ProductId);
}