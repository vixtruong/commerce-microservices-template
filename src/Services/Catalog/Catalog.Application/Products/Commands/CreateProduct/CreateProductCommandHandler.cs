using Catalog.Application.Abstractions;
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
            throw new NotImplementedException();
        }
    }
}
