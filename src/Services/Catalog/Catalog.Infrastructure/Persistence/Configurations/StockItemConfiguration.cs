using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures inventory balances and their relationship to products and reservations.
/// </summary>
internal sealed class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    /// <summary>
    /// Configures the stock item table, product key, constraints, and reservation collection.
    /// </summary>
    /// <param name="builder">The StockItem entity type builder.</param>
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("StockItems");

        builder.HasKey(stockItem => stockItem.Id);

        builder.Property(stockItem => stockItem.Id)
            .HasConversion(
                productId => productId.Value,
                value => ProductId.From(value))
            .HasColumnName("ProductId")
            .ValueGeneratedNever();

        builder.Property(stockItem => stockItem.QuantityOnHand)
            .IsRequired();

        builder.Property(stockItem => stockItem.ReservedQuantity)
            .IsRequired();

        builder.Property(stockItem => stockItem.CreatedAtUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(stockItem => stockItem.UpdatedAtUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Ignore(stockItem => stockItem.AvailableQuantity);
        builder.Ignore(stockItem => stockItem.DomainEvents);

        // A stock balance cannot exist without its Catalog product.
        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<StockItem>(stockItem => stockItem.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(stockItem => stockItem.Reservations)
            .WithOne()
            .HasForeignKey(reservation => reservation.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(stockItem => stockItem.Reservations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
