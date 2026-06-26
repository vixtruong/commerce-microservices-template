using Commerce.BuildingBlocks.Infrastructure.Logging;
using Commerce.Gateway.GraphQL.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommerceSerilog();
builder.Services.AddCatalogGateway(builder.Configuration);

var app = builder.Build();

app.UseCommerceRequestLogging();
app.MapGraphQL("/graphql");

app.Run();
