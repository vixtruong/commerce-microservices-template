using Catalog.Domain.Products.Events;
using Commerce.BuildingBlocks.Domain.Entities;
using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Products
{
    public class Product : AggregateRoot<ProductId>
    {
        private Product(
            ProductId id,
            string name,
            decimal price)
            : base(id)
        {
            Name = name;
            Price = price;
            Status = ProductStatus.Draft;
        }

        private Product() { }

        public string Name { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public ProductStatus Status { get; private set; }

        public static Result<Product> Create(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ProductErrors.NameRequired;
            }

            if (price <= 0)
            {
                return ProductErrors.PriceMustBePositive;
            }

            var product = new Product(ProductId.New(), name.Trim(), price);

            product.RaiseDomainEvent(
                new ProductCreatedDomainEvent(product.Id.Value, product.Name, product.Price));

            return product;
        }

        public Result ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                return ProductErrors.PriceMustBePositive;
            }

            if (Price == newPrice)
            {
                return Result.Success();
            }

            Price = newPrice;

            return Result.Success();
        }
    }
}
