using Commerce.Gateway.GraphQL.Catalog;
using HotChocolate.Execution.Configuration;

namespace Commerce.Gateway.GraphQL
{
    /// <summary>
    /// Registers the gateway GraphQL schema and service-specific GraphQL extensions.
    /// </summary>
    public static class CommerceGraphQLExtensions
    {
        /// <summary>
        /// Adds the Commerce GraphQL server and all service-backed schema extensions.
        /// </summary>
        /// <param name="services">Dependency injection service collection.</param>
        /// <param name="configuration">Gateway configuration containing downstream service addresses.</param>
        /// <returns>The configured service collection.</returns>
        public static IServiceCollection AddCommerceGraphQL(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            IRequestExecutorBuilder graphQlBuilder = services
                .AddGraphQLServer()
                .AddQueryType()
                .AddMutationType();

            graphQlBuilder.AddCatalogGraphQL(configuration);

            return services;
        }
    }
}
