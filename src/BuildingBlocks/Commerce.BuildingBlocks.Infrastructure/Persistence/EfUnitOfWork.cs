using Commerce.BuildingBlocks.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BuildingBlocks.Infrastructure.Persistence;

/// <summary>
/// Implements a unit of work over a scoped Entity Framework Core database context.
/// </summary>
/// <typeparam name="TDbContext">The service-owned database context type.</typeparam>
public sealed class EfUnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfUnitOfWork{TDbContext}"/> class.
    /// </summary>
    /// <param name="dbContext">The scoped database context to commit.</param>
    public EfUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
