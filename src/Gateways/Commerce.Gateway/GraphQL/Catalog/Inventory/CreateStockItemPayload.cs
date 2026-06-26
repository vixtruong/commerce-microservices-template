namespace Commerce.Gateway.GraphQL.Catalog.Inventory
{
    /// <summary>
    /// Represents the GraphQL payload returned after creating a stock item.
    /// </summary>
    /// <param name="ProductId">Identifier of the product associated with the created stock item.</param>
    public sealed record CreateStockItemPayload(Guid ProductId);
}