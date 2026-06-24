using Catalog.Application.Abstractions;
using Catalog.Domain.Inventory;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implements StockItem persistence with Entity Framework Core.
    /// </summary>
    public sealed class StockItemRepository : IStockItemRepository
    {
        private readonly CatalogDbContext _dbContext;

        public StockItemRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public void Add(StockItem stockItem)
        {
            _dbContext.StockItems.Add(stockItem);
        }

        /// <inheritdoc />
        public Task<StockItem?> GetByProductIdAsync(
            ProductId productId,
            CancellationToken cancellationToken)
        {
            // Reservations are required because the aggregate methods search and
            // modify reservation entities through the private backing collection.
            return _dbContext.StockItems
                .Include(stockItem => stockItem.Reservations)
                .SingleOrDefaultAsync(
                    stockItem => stockItem.Id == productId,
                    cancellationToken);
        }
    }
}
