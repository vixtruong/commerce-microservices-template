using Catalog.Contracts.Grpc;
using Commerce.Gateway.GraphQL.Catalog.Products;
using HotChocolate.Execution.Configuration;

namespace Commerce.Gateway.GraphQL.Catalog
{
    /// <summary>
    /// Registers Catalog gateway dependencies and GraphQL types.
    /// </summary>
    public static class CatalogGatewayExtensions
    {
        private const string CatalogGrpcClientConfigurationKey = "GrpcClients:Catalog";

        /// <summary>
        /// Adds Catalog gRPC clients and GraphQL schema extensions to the gateway schema.
        /// </summary>
        /// <param name="builder">GraphQL request executor builder.</param>
        /// <param name="configuration">Gateway configuration containing downstream service addresses.</param>
        /// <returns>The configured GraphQL request executor builder.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the Catalog gRPC client address is missing.
        /// </exception>
        public static IRequestExecutorBuilder AddCatalogGraphQL(
            this IRequestExecutorBuilder builder,
            IConfiguration configuration)
        {
            builder.Services
                .AddGrpcClient<CatalogGrpc.CatalogGrpcClient>(options =>
                {
                    string catalogAddress = configuration[CatalogGrpcClientConfigurationKey]
                        ?? throw new InvalidOperationException(
                            $"{CatalogGrpcClientConfigurationKey} was not configured.");

                    options.Address = new Uri(catalogAddress);
                });

            builder
                .AddTypeExtension<ProductQueries>()
                .AddTypeExtension<ProductMutations>();

            return builder;
        }
    }
}
