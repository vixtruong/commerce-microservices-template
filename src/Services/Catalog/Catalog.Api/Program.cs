using Catalog.Api.Services;
using Catalog.Infrastructure;
using Commerce.BuildingBlocks.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommerceSerilog();

builder.Services.AddGrpc();
builder.Services.AddCatalogInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseCommerceRequestLogging();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
