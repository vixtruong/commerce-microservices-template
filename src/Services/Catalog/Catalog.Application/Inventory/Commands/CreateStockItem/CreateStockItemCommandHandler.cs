using Catalog.Application.Abstractions;
using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Application.Persistence;
using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Inventory.Commands.CreateStockItem
{
    /// <summary>
    /// Handles creation of stock items for Catalog products.
    /// </summary>
    public sealed class CreateStockItemCommandHandler : IRequestHandler<CreateStockItemCommand, Result<CreateStockItemResponse>>
    {
        private readonly IStockItemRepository _stockItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStockItemCommandHandler(
            IStockItemRepository stockItemRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _stockItemRepository = stockItemRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStockItemCommandHandler"/> class.
        /// </summary>
        /// <param name="stockItemRepository">Repository used to persist stock item aggregates.</param>
        /// <param name="productRepository">Repository used to verify the related product exists.</param>
        /// <param name="unitOfWork">Unit of work used to commit stock item changes.</param>
        public async Task<Result<CreateStockItemResponse>> Handle(
            CreateStockItemCommand request,
            CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
            {
                return Result<CreateStockItemResponse>.Failure(InventoryErrors.ProductIdRequired);
            }

            ProductId productId = ProductId.From(request.ProductId);

            bool productExists = await _productRepository.ExistsByIdAsync(
                productId,
                cancellationToken);

            if (!productExists)
            {
                return Result<CreateStockItemResponse>.Failure(
                    ProductErrors.NotFound(productId));
            }

            StockItem? existingStockItem = await _stockItemRepository.GetByProductIdAsync(
                productId,
                cancellationToken);

            if (existingStockItem is not null)
            {
                return Result<CreateStockItemResponse>.Failure(
                    InventoryErrors.StockItemAlreadyExists(request.ProductId));
            }

            StockItem stockItem = StockItem.Create(productId, DateTimeOffset.UtcNow);

            _stockItemRepository.Add(stockItem);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateStockItemResponse(productId.Value);
        }
    }
}
