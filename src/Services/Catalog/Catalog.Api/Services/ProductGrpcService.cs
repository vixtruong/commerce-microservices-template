using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Queries.GetProductById;
using Catalog.Contracts.Grpc.Products;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Api.Grpc;
using Commerce.BuildingBlocks.Domain.Results;
using Grpc.Core;
using MediatR;
using System.Globalization;

namespace Catalog.Api.Services
{
    /// <summary>
    /// Exposes Catalog application use cases through the internal gRPC contract.
    /// </summary>
    public sealed class ProductGrpcService : ProductGrpc.ProductGrpcBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductGrpcService"/> class.
        /// </summary>
        /// <param name="sender">MediatR sender used to execute Catalog commands and queries.</param>
        public ProductGrpcService(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Gets one product by its identifier.
        /// </summary>
        /// <param name="request">gRPC request containing the product identifier.</param>
        /// <param name="context">gRPC server call context.</param>
        /// <returns>The matching product reply.</returns>
        public override async Task<ProductReply> GetProductById(
            GetProductByIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.ProductId, out Guid productId))
            {
                throw GrpcErrorMapper.ToRpcException(ProductErrors.IdInvalid);
            }

            Result<ProductResponse> result = await _sender.Send(
                new GetProductByIdQuery(productId),
                context.CancellationToken);

            if (result.IsFailure)
            {
                throw GrpcErrorMapper.ToRpcException(result.Error);
            }

            return MapToReply(result.Value);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">gRPC request containing product creation data.</param>
        /// <param name="context">gRPC server call context.</param>
        /// <returns>The identifier of the created product.</returns>
        public override async Task<CreateProductReply> CreateProduct(
            CreateProductRequest request,
            ServerCallContext context)
        {
            if (!decimal.TryParse(request.PriceAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal priceAmount))
            {
                throw GrpcErrorMapper.ToRpcException(ProductErrors.PriceAmountInvalid);
            }

            Result<CreateProductResponse> result = await _sender.Send(
                new CreateProductCommand(
                    request.Sku,
                    request.Name,
                    request.Description,
                    priceAmount,
                    request.PriceCurrency),
                context.CancellationToken);

            if (result.IsFailure)
            {
                throw GrpcErrorMapper.ToRpcException(result.Error);
            }

            return new CreateProductReply
            {
                ProductId = result.Value.ProductId.ToString()
            };
        }

        /// <summary>
        /// Maps application product data to the public gRPC reply.
        /// </summary>
        /// <param name="product">Application product response.</param>
        /// <returns>The gRPC product reply.</returns>
        private static ProductReply MapToReply(ProductResponse product)
        {
            return new ProductReply
            {
                Id = product.Id.ToString(),
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description,
                PriceAmount = product.PriceAmount.ToString(CultureInfo.InvariantCulture),
                PriceCurrency = product.PriceCurrency,
                Status = product.Status
            };
        }
    }
}
