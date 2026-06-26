using Catalog.Application.Abstractions;
using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Inventory.Queries.GetStockItemByProductId
{
    /// <summary>
    /// Handles retrieval of stock information for a specific Catalog product.
    /// </summary>
    public sealed class GetStockItemByProductIdQueryHandler
        : IRequestHandler<GetStockItemByProductIdQuery, Result<StockItemResponse>>
    {
        private readonly IStockItemRepository _stockItemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStockItemByProductIdQueryHandler"/> class.
        /// </summary>
        /// <param name="stockItemRepository">Repository used to read stock item aggregates.</param>
        public GetStockItemByProductIdQueryHandler(
            IStockItemRepository stockItemRepository)
        {
            _stockItemRepository = stockItemRepository;
        }

        /// <summary>
        /// Gets stock information for the requested product.
        /// </summary>
        /// <param name="request">Query containing the product identifier.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>
        /// Returns stock item information when found; otherwise, returns a validation or not-found error.
        /// </returns>
        public async Task<Result<StockItemResponse>> Handle(
            GetStockItemByProductIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
            {
                return Result<StockItemResponse>.Failure(InventoryErrors.ProductIdRequired);
            }

            ProductId productId = ProductId.From(request.ProductId);

            StockItem? stockItem = await _stockItemRepository.GetByProductIdAsync(
                productId,
                cancellationToken);

            if (stockItem is null)
            {
                return Result<StockItemResponse>.Failure(
                    InventoryErrors.StockItemNotFound(request.ProductId));
            }

            StockItemResponse response = new(
                stockItem.Id.Value,
                stockItem.QuantityOnHand,
                stockItem.ReservedQuantity,
                stockItem.AvailableQuantity,
                stockItem.CreatedAtUtc,
                stockItem.UpdatedAtUtc);

            return response;
        }
    }
}
