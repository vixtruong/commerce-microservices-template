using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures stock reservations created by the inventory aggregate.
/// </summary>
internal sealed class StockReservationConfiguration : IEntityTypeConfiguration<StockReservation>
{
    /// <summary>
    /// Configures reservation keys, strongly typed identifiers, constraints, and indexes.
    /// </summary>
    /// <param name="builder">The StockReservation entity type builder.</param>
    public void Configure(EntityTypeBuilder<StockReservation> builder)
    {
        builder.ToTable("StockReservations");

        builder.HasKey(reservation => reservation.Id);

        builder.Property(reservation => reservation.Id)
            .HasConversion(
                reservationId => reservationId.Value,
                value => StockReservationId.From(value))
            .ValueGeneratedNever();

        builder.Property(reservation => reservation.ProductId)
            .HasConversion(
                productId => productId.Value,
                value => ProductId.From(value))
            .IsRequired();

        builder.Property(reservation => reservation.OrderId)
            .IsRequired();

        builder.Property(reservation => reservation.Quantity)
            .IsRequired();

        builder.Property(reservation => reservation.Status)
            // Persist the enum name so operational reservation states are self-describing in the database.
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(reservation => reservation.CreatedAtUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(reservation => reservation.ExpiresAtUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(reservation => reservation.CompletedAtUtc)
            .HasPrecision(0);

        builder.Property(reservation => reservation.CompletionReason)
            .HasMaxLength(500);

        // The index supports idempotency checks and order-centric reservation lookup.
        builder.HasIndex(reservation => new { reservation.OrderId, reservation.ProductId });
    }
}
