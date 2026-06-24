using Catalog.Application.Abstractions;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Application.Persistence;
using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct
{
    /// <summary>
    /// Creates a Product aggregate and commits it through the unit of work.
    /// </summary>
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates and persists a new Product aggregate.
        /// </summary>
        /// <param name="request">Product creation command.</param>
        /// <param name="cancellationToken">
        /// Token used to cancel the operation.
        /// </param>
        /// <returns>The created product identifier or a validation error.</returns>
        public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            string normalizedSku = request.Sku.Trim().ToUpperInvariant();

            bool skuExists = await _productRepository.ExistsBySkuAsync(normalizedSku, cancellationToken);

            if (skuExists)
            {
                return Result<CreateProductResponse>.Failure(
                    Error.Conflict(
                        "Catalog.Product.SkuAlreadyExists",
                        $"Product SKU '{normalizedSku}' already exists."));
            }

            Result<Product> productResult = Product.Create(
                normalizedSku,
                request.Name,
                request.Description,
                request.PriceAmount,
                request.PriceCurrency,
                DateTimeOffset.UtcNow);

            if (productResult.IsFailure)
            {
                return productResult.Error;
            }

            Product product = productResult.Value;

            _productRepository.Add(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse(product.Id.Value);
        }
    }
}
