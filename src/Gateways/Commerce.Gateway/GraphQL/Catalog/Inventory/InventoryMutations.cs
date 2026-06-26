using Catalog.Contracts.Grpc.Inventory;
using Commerce.Gateway.GraphQL.Common;
using Grpc.Core;

namespace Commerce.Gateway.GraphQL.Catalog.Inventory
{
    /// <summary>
    /// GraphQL mutations for Catalog inventory write operations.
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public sealed class InventoryMutations
    {
        /// <summary>
        /// Creates a stock item for an existing Catalog product.
        /// </summary>
        /// <param name="input">Input data containing the product identifier.</param>
        /// <param name="client">Inventory gRPC client used to call the Catalog service.</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>
        /// Returns the created stock item payload.
        /// </returns>
        public async Task<CreateStockItemPayload> CreateStockItemAsync(
            CreateStockItemInput input,
            [Service] InventoryGrpc.InventoryGrpcClient client,
            CancellationToken cancellationToken)
        {
            try
            {
                CreateStockItemReply reply = await client.CreateStockItemAsync(
                    new CreateStockItemRequest
                    {
                        ProductId = input.ProductId.ToString(),
                    },
                    cancellationToken: cancellationToken);

                return MapToPayload(reply);
            }
            catch (RpcException exception)
            {
                throw GraphQlGrpcErrorMapper.ToGraphQlException(exception);
            }
        }

        /// <summary>
        /// Maps the create stock item reply returned by Catalog gRPC into the GraphQL payload.
        /// </summary>
        /// <param name="reply">Create stock item reply returned by the Catalog service.</param>
        /// <returns>
        /// Returns the GraphQL payload exposed by the Gateway schema.
        /// </returns>
        private static CreateStockItemPayload MapToPayload(CreateStockItemReply reply)
        {
            return new CreateStockItemPayload(Guid.Parse(reply.ProductId));
        }
    }
}
