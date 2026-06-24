using Catalog.Api.Services;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Extensions;
using Commerce.BuildingBlocks.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommerceSerilog();

builder.Services.AddGrpc();
builder.Services.AddCatalogApplication(builder.Configuration["MediatR:LicenseKey"]);
builder.Services.AddCatalogInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    await app.Services.MigrateCatalogDatabaseAsync();
}

app.UseCommerceRequestLogging();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();
