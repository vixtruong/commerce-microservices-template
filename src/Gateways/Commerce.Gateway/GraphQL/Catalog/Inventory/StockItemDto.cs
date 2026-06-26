namespace Commerce.Gateway.GraphQL.Catalog.Inventory
{
    /// <summary>
    /// Represents Catalog inventory stock information returned through the GraphQL gateway.
    /// </summary>
    /// <param name="ProductId">Identifier of the product associated with the stock item.</param>
    /// <param name="QuantityOnHand">Total quantity physically available in stock.</param>
    /// <param name="ReservedQuantity">Quantity currently reserved for pending orders.</param>
    /// <param name="AvailableQuantity">Quantity available for new reservations.</param>
    public sealed record StockItemDto(
        Guid ProductId,
        int QuantityOnHand,
        int ReservedQuantity,
        int AvailableQuantity);
}