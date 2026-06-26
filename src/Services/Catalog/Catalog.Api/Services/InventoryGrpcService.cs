using Catalog.Application.Inventory.Commands.CreateStockItem;
using Catalog.Application.Inventory.Queries.GetStockItemByProductId;
using Catalog.Contracts.Grpc.Inventory;
using Catalog.Domain.Inventory;
using Commerce.BuildingBlocks.Api.Grpc;
using Grpc.Core;
using MediatR;

namespace Catalog.Api.Services
{
    /// <summary>
    /// Exposes Catalog inventory use cases through the internal gRPC contract.
    /// </summary>
    public sealed class InventoryGrpcService : InventoryGrpc.InventoryGrpcBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryGrpcService"/> class.
        /// </summary>
        /// <param name="sender">MediatR sender used to execute inventory commands and queries.</param>
        public InventoryGrpcService(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Gets stock information for a Catalog product.
        /// </summary>
        /// <param name="request">Request containing the product identifier.</param>
        /// <param name="context">gRPC server call context.</param>
        /// <returns>The stock item reply for the requested product.</returns>
        public override async Task<StockItemReply> GetStockItemByProductId(
            GetStockItemByProductIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.ProductId, out Guid productId))
            {
                throw GrpcErrorMapper.ToRpcException(InventoryErrors.ProductIdInvalid);
            }

            var result = await _sender.Send(
                new GetStockItemByProductIdQuery(productId),
                context.CancellationToken);

            if (result.IsFailure)
            {
                throw GrpcErrorMapper.ToRpcException(result.Error);
            }

            return MapToStockItemReply(result.Value);
        }

        /// <summary>
        /// Creates an empty stock item for a Catalog product.
        /// </summary>
        /// <param name="request">Request containing the product identifier.</param>
        /// <param name="context">gRPC server call context.</param>
        /// <returns>The created stock item reply.</returns>
        public override async Task<CreateStockItemReply> CreateStockItem(
            CreateStockItemRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.ProductId, out Guid productId))
            {
                throw GrpcErrorMapper.ToRpcException(InventoryErrors.ProductIdInvalid);
            }

            var result = await _sender.Send(
                new CreateStockItemCommand(productId),
                context.CancellationToken);

            if (result.IsFailure)
            {
                throw GrpcErrorMapper.ToRpcException(result.Error);
            }

            return new CreateStockItemReply
            {
                ProductId = result.Value.ProductId.ToString()
            };
        }

        /// <summary>
        /// Maps application stock item data to the inventory gRPC reply.
        /// </summary>
        /// <param name="stockItem">Application stock item response.</param>
        /// <returns>The gRPC stock item reply.</returns>
        private static StockItemReply MapToStockItemReply(StockItemResponse stockItem)
        {
            return new StockItemReply
            {
                ProductId = stockItem.ProductId.ToString(),
                QuantityOnHand = stockItem.QuantityOnHand,
                ReservedQuantity = stockItem.ReservedQuantity,
                AvailableQuantity = stockItem.AvailableQuantity
            };
        }
    }
}
