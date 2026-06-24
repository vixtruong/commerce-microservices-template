using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Commerce.BuildingBlocks.Infrastructure.Logging;

/// <summary>
/// Provides consistent Serilog host and HTTP request logging for Commerce services.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Replaces the default logging providers with a shared Serilog configuration.
    /// Service-specific minimum levels can be overridden through the Serilog configuration section.
    /// </summary>
    /// <param name="builder">The web application builder to configure.</param>
    /// <returns>The same builder so additional host configuration can be chained.</returns>
    public static WebApplicationBuilder AddCommerceSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Application}] {Message:lj}{NewLine}{Exception}");
        });

        return builder;
    }

    /// <summary>
    /// Adds one structured completion log for each HTTP or gRPC request.
    /// </summary>
    /// <param name="app">The application whose request pipeline is being configured.</param>
    /// <returns>The same application so middleware registration can be chained.</returns>
    public static WebApplication UseCommerceRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                // Query strings are intentionally excluded because they can contain credentials
                // or other client-supplied sensitive values.
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            };
        });

        return app;
    }
}
