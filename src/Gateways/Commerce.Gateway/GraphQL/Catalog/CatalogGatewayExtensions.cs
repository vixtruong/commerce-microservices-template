using Catalog.Contracts.Grpc;
using Commerce.Gateway.GraphQL.Catalog.Products;

namespace Commerce.Gateway.GraphQL.Catalog
{
    /// <summary>
    /// Registers Catalog gateway dependencies and GraphQL types.
    /// </summary>
    public static class CatalogGatewayExtensions
    {
        private const string CatalogGrpcClientConfigurationKey = "GrpcClients:Catalog";

        /// <summary>
        /// Adds Catalog gRPC clients and GraphQL schema types to the gateway.
        /// </summary>
        /// <param name="services">Dependency injection service collection.</param>
        /// <param name="configuration">Gateway configuration containing downstream service addresses.</param>
        /// <returns>The configured service collection.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the Catalog gRPC client address is missing.
        /// </exception>
        public static IServiceCollection AddCatalogGateway(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddGrpcClient<CatalogGrpc.CatalogGrpcClient>(options =>
                {
                    string catalogAddress = configuration[CatalogGrpcClientConfigurationKey]
                        ?? throw new InvalidOperationException(
                            $"{CatalogGrpcClientConfigurationKey} was not configured.");

                    options.Address = new Uri(catalogAddress);
                });

            services
                .AddGraphQLServer()
                .AddQueryType<ProductQueries>()
                .AddMutationType<ProductMutations>();

            return services;
        }
    }
}
