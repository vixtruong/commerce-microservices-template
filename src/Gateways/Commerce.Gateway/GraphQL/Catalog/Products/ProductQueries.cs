using Catalog.Contracts.Grpc;
using Commerce.Gateway.GraphQL.Common;
using Grpc.Core;
using HotChocolate.Types;
using System.Globalization;

namespace Commerce.Gateway.GraphQL.Catalog.Products
{
    /// <summary>
    /// GraphQL queries for Catalog product read operations.
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public sealed class ProductQueries
    {

        /// <summary>
        /// Gets one product from the Catalog service.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="client">Catalog gRPC client.</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>The product when found; otherwise null.</returns>
        public async Task<ProductDto?> GetProductByIdAsync(
            Guid productId,
            [Service] CatalogGrpc.CatalogGrpcClient client,
            CancellationToken cancellationToken)
        {
            try
            {
                ProductReply reply = client.GetProductById(
                    new GetProductByIdRequest { ProductId = productId.ToString() },
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
        /// Maps a gRPC product reply to a GraphQL DTO.
        /// </summary>
        /// <param name="reply">Product reply returned by Catalog gRPC.</param>
        /// <returns>The GraphQL product DTO.</returns>
        private static ProductDto MapToDto(ProductReply reply)
        {
            return new ProductDto(
                Guid.Parse(reply.Id),
                reply.Sku,
                reply.Name,
                reply.Description,
                decimal.Parse(reply.PriceAmount, CultureInfo.InvariantCulture),
                reply.PriceCurrency,
                reply.Status);
        }
    }
}
