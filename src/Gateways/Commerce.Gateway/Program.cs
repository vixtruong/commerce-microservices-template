using Commerce.BuildingBlocks.Infrastructure.Logging;
using Commerce.Gateway.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommerceSerilog();
builder.Services.AddCommerceGraphQL(builder.Configuration);

var app = builder.Build();

app.UseCommerceRequestLogging();
app.MapGraphQL("/graphql");

app.Run();
