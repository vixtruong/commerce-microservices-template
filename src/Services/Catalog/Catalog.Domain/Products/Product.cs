using Catalog.Domain.Common;
using Catalog.Domain.Products.Events;
using Commerce.BuildingBlocks.Domain.Entities;
using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Products
{
    public class Product : AggregateRoot<ProductId>
    {
        private Product(
            ProductId id,
            string sku,
            string name,
            string description,
            Money price,
            DateTimeOffset createdAtUtc)
            : base(id)
        {
            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            Status = ProductStatus.Draft;
            CreatedAtUtc = createdAtUtc;
        }

        private Product() { }

        public string Sku { get; private set; } = string.Empty;

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public Money Price { get; private set; } = null!;

        public ProductStatus Status { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset UpdatedAtUtc { get; private set; }

        public static Result<Product> Create(
            string sku,
            string name,
            string? description,
            decimal priceAmount,
            string priceCurrency,
            DateTimeOffset createdAtUtc)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return ProductErrors.SkuRequired;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return ProductErrors.NameRequired;
            }

            Result<Money> priceResult = Money.Create(priceAmount, priceCurrency);

            if (priceResult.IsFailure)
            {
                return priceResult.Error;
            }

            var product = new Product(
                ProductId.New(),
                sku.Trim().ToUpperInvariant(),
                name.Trim(),
                description?.Trim() ?? string.Empty,
                priceResult.Value,
                createdAtUtc);

            product.RaiseDomainEvent(
                new ProductCreatedDomainEvent(
                    product.Id.Value,
                    product.Sku,
                    product.Name));

            return product;
        }

        public Result Rename(string name, DateTimeOffset changedAtUtc)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ProductErrors.NameRequired;
            }

            Name = name.Trim();
            UpdatedAtUtc = changedAtUtc;
            return Result.Success();
        }

        public Result ChangeDescription(string? description, DateTimeOffset changedAtUtc)
        {
            Description = description?.Trim() ?? string.Empty;
            UpdatedAtUtc = changedAtUtc;
            return Result.Success();
        }

        public Result ChangePrice(
            decimal amount,
            string currency,
            DateTimeOffset changedAtUtc)
        {
            Result<Money> priceResult = Money.Create(amount, currency);

            if (priceResult.IsFailure)
            {
                return priceResult.Error;
            }

            Money newPrice = priceResult.Value;

            if (Price.Equals(newPrice))
            {
                return Result.Success();
            }

            decimal previousAmount = Price.Amount;
            Price = newPrice;
            UpdatedAtUtc = changedAtUtc;

            RaiseDomainEvent(
                new ProductPriceChangedDomainEvent(
                    Id.Value,
                    previousAmount,
                    Price.Amount,
                    Price.Currency));

            return Result.Success();
        }

        public Result Activate(DateTimeOffset changedAtUtc)
        {
            if (Status == ProductStatus.Active)
            {
                return Result.Success();
            }

            Status = ProductStatus.Active;
            UpdatedAtUtc = changedAtUtc;
            return Result.Success();
        }

        public Result Deactivate(DateTimeOffset changedAtUtc)
        {
            if (Status == ProductStatus.Inactive)
            {
                return Result.Success();
            }

            Status = ProductStatus.Inactive;
            UpdatedAtUtc = changedAtUtc;
            return Result.Success();
        }
    }
}
