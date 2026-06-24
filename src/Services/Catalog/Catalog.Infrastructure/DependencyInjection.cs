using Catalog.Application.Abstractions;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Repositories;
using Commerce.BuildingBlocks.Application.Persistence;
using Commerce.BuildingBlocks.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

/// <summary>
/// Registers Catalog infrastructure services.
/// </summary>
public static class DependencyInjection
{
    private const string ConnectionStringName = "CatalogDb";

    /// <summary>
    /// Adds Catalog persistence and repository implementations.
    /// </summary>
    /// <param name="services">Dependency injection service collection.</param>
    /// <param name="configuration">
    /// Host configuration containing the Catalog connection string.
    /// </param>
    /// <returns>The configured service collection.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Catalog connection string is missing.
    /// </exception>
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' " +
                "was not configured.");

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(
                        typeof(CatalogDbContext).Assembly.FullName);

                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3);
                });
        });

        services.AddScoped<
            IUnitOfWork,
            EfUnitOfWork<CatalogDbContext>>();

        services.AddScoped<
            IProductRepository,
            ProductRepository>();

        services.AddScoped<
            IStockItemRepository,
            StockItemRepository>();

        return services;
    }
}
