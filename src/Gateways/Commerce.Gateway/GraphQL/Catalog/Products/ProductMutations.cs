using Catalog.Contracts.Grpc;
using Commerce.Gateway.GraphQL.Common;
using Grpc.Core;
using System.Globalization;

namespace Commerce.Gateway.GraphQL.Catalog.Products
{
    /// <summary>
    /// GraphQL mutations for Catalog product write operations.
    /// </summary>
    public sealed class ProductMutations
    {
        /// <summary>
        /// Creates a product through the Catalog service.
        /// </summary>
        /// <param name="input">Product creation input.</param>
        /// <param name="client">Catalog gRPC client.</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>The created product payload.</returns>
        public async Task<CreateProductPayload> CreateProductAsync(
            CreateProductInput input,
            [Service] CatalogGrpc.CatalogGrpcClient client,
            CancellationToken cancellationToken)
        {
            try
            {
                CreateProductReply reply = client.CreateProduct(
                    new CreateProductRequest
                    {
                        Sku = input.Sku,
                        Name = input.Name,
                        Description = input.Description,
                        PriceAmount = input.PriceAmount.ToString(CultureInfo.InvariantCulture),
                        PriceCurrency = input.PriceCurrency
                    },
                    cancellationToken: cancellationToken);

                return new CreateProductPayload(Guid.Parse(reply.ProductId));
            }
            catch (RpcException exception)
            {
                throw GraphQlGrpcErrorMapper.ToGraphQlException(exception);
            }
        }
    }
}
