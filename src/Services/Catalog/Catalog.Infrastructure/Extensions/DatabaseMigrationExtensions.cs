using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Extensions
{
    /// <summary>
    /// Provides database initialization methods for the Catalog service.
    /// </summary>
    public static class DatabaseMigrationExtensions
    {
        /// <summary>
        /// Applies all pending Entity Framework Core migrations
        /// for the Catalog database.
        /// </summary>
        /// <param name="serviceProvider">
        /// The root application service provider.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token used to stop the migration operation.
        /// </param>
        public static async Task MigrateCatalogDatabaseAsync(
            this IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            await using AsyncServiceScope scope =
                serviceProvider.CreateAsyncScope();

            CatalogDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            ILogger<CatalogDbContext> logger =
                scope.ServiceProvider
                    .GetRequiredService<ILogger<CatalogDbContext>>();

            try
            {
                logger.LogInformation(
                    "Applying pending Catalog database migrations.");

                await dbContext.Database.MigrateAsync(cancellationToken);

                logger.LogInformation(
                    "Catalog database migrations applied successfully.");
            }
            catch (Exception exception)
            {
                logger.LogCritical(
                    exception,
                    "An error occurred while migrating the Catalog database.");

                throw;
            }
        }
    }
}
