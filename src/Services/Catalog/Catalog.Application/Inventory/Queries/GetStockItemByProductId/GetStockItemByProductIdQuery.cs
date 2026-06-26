using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Inventory.Queries.GetStockItemByProductId
{
    /// <summary>
    /// Requests inventory stock information for a specific Catalog product.
    /// </summary>
    /// <param name="ProductId">Identifier of the product whose stock item should be returned.</param>
    public sealed record GetStockItemByProductIdQuery(Guid ProductId)
        : IRequest<Result<StockItemResponse>>;

    /// <summary>
    /// Represents inventory stock information returned by the application layer.
    /// </summary>
    /// <param name="ProductId">Identifier of the product associated with the stock item.</param>
    /// <param name="QuantityOnHand">Total quantity physically available in stock.</param>
    /// <param name="ReservedQuantity">Quantity currently reserved for pending orders.</param>
    /// <param name="AvailableQuantity">Quantity available for new reservations.</param>
    /// <param name="CreatedAtUtc">UTC date and time when the stock item was created.</param>
    /// <param name="UpdatedAtUtc">UTC date and time when the stock item was last updated.</param>
    public sealed record StockItemResponse(
        Guid ProductId,
        int QuantityOnHand,
        int ReservedQuantity,
        int AvailableQuantity,
        DateTimeOffset CreatedAtUtc,
        DateTimeOffset UpdatedAtUtc);
}