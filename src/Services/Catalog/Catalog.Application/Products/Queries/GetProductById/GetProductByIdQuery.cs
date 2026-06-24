using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Products.Queries.GetProductById
{
    /// <summary>
    /// Requests one product by its identifier.
    /// </summary>
    /// <param name="ProductId">Product identifier.</param>
    public sealed record GetProductByIdQuery(Guid ProductId)
        : IRequest<Result<ProductResponse>>;

    /// <summary>
    /// Represents product data returned by the application.
    /// </summary>
    /// <param name="Id">Product identifier.</param>
    /// <param name="Sku">Unique SKU.</param>
    /// <param name="Name">Display name.</param>
    /// <param name="Description">Description.</param>
    /// <param name="PriceAmount">Price amount.</param>
    /// <param name="PriceCurrency">Price currency.</param>
    /// <param name="Status">Product status.</param>
    /// <param name="CreatedAtUtc">UTC creation time.</param>
    /// <param name="UpdatedAtUtc">UTC update time.</param>
    public sealed record ProductResponse(
        Guid Id,
        string Sku,
        string Name,
        string Description,
        decimal PriceAmount,
        string PriceCurrency,
        string Status,
        DateTimeOffset CreatedAtUtc,
        DateTimeOffset UpdatedAtUtc);
}
