using Catalog.Contracts.Grpc.Inventory;
using Commerce.Gateway.GraphQL.Common;
using Grpc.Core;

namespace Commerce.Gateway.GraphQL.Catalog.Inventory
{
    /// <summary>
    /// GraphQL queries for Catalog inventory read operations.
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public sealed class InventoryQueries
    {
        /// <summary>
        /// Gets the stock item of a Catalog product by product identifier.
        /// </summary>
        /// <param name="productId">Identifier of the product whose stock item should be returned.</param>
        /// <param name="client">Inventory gRPC client used to call the Catalog service.</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>
        /// Returns the stock item when found; otherwise, returns null.
        /// </returns>
        public async Task<StockItemDto?> GetStockItemByProductIdAsync(
            Guid productId,
            [Service] InventoryGrpc.InventoryGrpcClient client,
            CancellationToken cancellationToken)
        {
            try
            {
                StockItemReply reply = await client.GetStockItemByProductIdAsync(
                    new GetStockItemByProductIdRequest { ProductId = productId.ToString() },
                    cancellationToken: cancellationToken);

                return MapToDto(reply);
            }
            catch (RpcException exception) when (exception.StatusCode == StatusCode.NotFound)
            {
                return null;
            }
            catch (RpcException exception)
            {
                throw GraphQlGrpcErrorMapper.ToGraphQlException(exception);
            }
        }

        /// <summary>
        /// Maps the stock item reply returned by Catalog gRPC into the GraphQL DTO.
        /// </summary>
        /// <param name="reply">Stock item data returned by the Catalog service.</param>
        /// <returns>
        /// Returns the stock item DTO exposed by the Gateway GraphQL schema.
        /// </returns>
        private static StockItemDto MapToDto(StockItemReply reply)
        {
            return new StockItemDto(
                Guid.Parse(reply.ProductId),
                reply.QuantityOnHand,
                reply.ReservedQuantity,
                reply.AvailableQuantity);
        }
    }
}
