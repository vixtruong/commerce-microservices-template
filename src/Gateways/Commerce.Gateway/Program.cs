using Commerce.BuildingBlocks.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommerceSerilog();

var app = builder.Build();

app.UseCommerceRequestLogging();

app.MapGet("/", () => "Hello World!");

app.Run();
