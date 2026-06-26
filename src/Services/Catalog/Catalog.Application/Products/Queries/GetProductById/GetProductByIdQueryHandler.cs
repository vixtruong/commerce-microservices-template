using Catalog.Application.Abstractions;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Gets one product by identifier.
        /// </summary>
        /// <param name="request">Product query.</param>
        /// <param name="cancellationToken">
        /// Token used to cancel the operation.
        /// </param>
        /// <returns>The product response or a not-found error.</returns>
        public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
            {
                return Result<ProductResponse>.Failure(ProductErrors.IdRequired);
            }

            ProductId productId = ProductId.From(request.ProductId);

            Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                return Result<ProductResponse>.Failure(ProductErrors.NotFound(productId));
            }

            ProductResponse response = new(
                product.Id.Value,
                product.Sku,
                product.Name,
                product.Description,
                product.Price.Amount,
                product.Price.Currency,
                product.Status.ToString(),
                product.CreatedAtUtc,
                product.UpdatedAtUtc);

            return response;
        }
    }
}
