using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct
{
    /// <summary>
    /// Requests creation of a Catalog product.
    /// </summary>
    /// <param name="Sku">Unique product SKU.</param>
    /// <param name="Name">Product display name.</param>
    /// <param name="Description">Optional product description.</param>
    /// <param name="PriceAmount">Initial price amount.</param>
    /// <param name="PriceCurrency">Three-character currency code.</param>
    public sealed record CreateProductCommand(
        string Sku,
        string Name,
        string? Description,
        decimal PriceAmount,
        string PriceCurrency)
        : IRequest<Result<CreateProductResponse>>;

    /// <summary>
    /// Represents the result returned after creating a product.
    /// </summary>
    /// <param name="ProductId">Identifier of the created product.</param>
    public sealed record CreateProductResponse(Guid ProductId);

}
