using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Commerce.BuildingBlocks.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

/// <summary>
/// Represents the Catalog service database and its aggregate persistence mappings.
/// </summary>
public sealed class CatalogDbContext : DomainEventsDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogDbContext"/> class.
    /// </summary>
    /// <param name="options">Options that configure the Catalog database provider.</param>
    /// <param name="publisher">Publisher used to dispatch aggregate domain events.</param>
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IPublisher publisher)
        : base(options, publisher)
    {
    }

    /// <summary>
    /// Gets the products tracked by the Catalog service.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// Gets inventory balances tracked for Catalog products.
    /// </summary>
    public DbSet<StockItem> StockItems => Set<StockItem>();

    /// <summary>
    /// Gets stock reservations created for orders.
    /// </summary>
    public DbSet<StockReservation> StockReservations => Set<StockReservation>();

    /// <summary>
    /// Applies all Catalog entity configurations from the infrastructure assembly.
    /// </summary>
    /// <param name="modelBuilder">Builder used to construct the Catalog persistence model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
