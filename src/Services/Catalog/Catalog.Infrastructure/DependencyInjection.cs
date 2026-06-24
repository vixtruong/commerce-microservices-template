using Catalog.Application;
using Catalog.Infrastructure.Persistence;
using Commerce.BuildingBlocks.Application.Persistence;
using Commerce.BuildingBlocks.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

/// <summary>
/// Registers Catalog infrastructure services with the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    private const string CatalogConnectionStringName = "CatalogDb";

    /// <summary>
    /// Adds the Catalog SQL Server context, unit of work, and application event handlers.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The host configuration containing the Catalog connection string.</param>
    /// <returns>The configured service collection.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <c>CatalogDb</c> connection string is missing.
    /// </exception>
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString(CatalogConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{CatalogConnectionStringName}' was not configured.");

        string? mediatRLicenseKey = configuration["MediatR:LicenseKey"];

        services.AddMediatR(mediatRConfiguration =>
        {
            mediatRConfiguration.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            // MediatR 14 requires a production license. Keeping the key in external
            // configuration prevents credentials from being committed to source control.
            if (!string.IsNullOrWhiteSpace(mediatRLicenseKey))
            {
                mediatRConfiguration.LicenseKey = mediatRLicenseKey;
            }
        });

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName);
                    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 3);
                }));

        services.AddScoped<IUnitOfWork, EfUnitOfWork<CatalogDbContext>>();

        return services;
    }
}
