using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application
{
    /// <summary>
    /// Registers Catalog application use cases and event handlers.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds Catalog application services.
        /// </summary>
        /// <param name="services">Dependency injection service collection.</param>
        /// <param name="mediatRLicenseKey">
        /// Optional MediatR production license key.
        /// </param>
        /// <returns>The configured service collection.</returns>
        public static IServiceCollection AddCatalogApplication(
            this IServiceCollection services,
            string? mediatRLicenseKey = null)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(
                    AssemblyReference.Assembly);

                if (!string.IsNullOrWhiteSpace(mediatRLicenseKey))
                {
                    configuration.LicenseKey = mediatRLicenseKey;
                }
            });

            return services;
        }
    }
}
