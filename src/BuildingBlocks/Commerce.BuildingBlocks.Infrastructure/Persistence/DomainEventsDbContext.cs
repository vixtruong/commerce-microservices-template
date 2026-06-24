using Commerce.BuildingBlocks.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BuildingBlocks.Infrastructure.Persistence;

/// <summary>
/// Provides an Entity Framework Core context that dispatches aggregate domain events
/// through MediatR before committing the associated state changes.
/// </summary>
public abstract class DomainEventsDbContext : DbContext
{
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventsDbContext"/> class.
    /// </summary>
    /// <param name="options">Options that configure the database context.</param>
    /// <param name="publisher">Publisher used to dispatch domain events.</param>
    protected DomainEventsDbContext(DbContextOptions options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    /// <summary>
    /// Dispatches pending domain events and then persists the tracked entity changes.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">
    /// Indicates whether Entity Framework Core should accept tracked changes after a successful save.
    /// </param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Publishes the domain events raised by all tracked aggregates.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel event dispatch.</param>
    /// <returns>A task that completes after every pending event has been published.</returns>
    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        IHasDomainEvents[] aggregates = ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count > 0)
            .ToArray();

        IDomainEvent[] domainEvents = aggregates
            .SelectMany(entity => entity.DomainEvents)
            .ToArray();

        // Clear before publication so a handler that saves through the same scoped context
        // cannot dispatch the same event recursively.
        foreach (IHasDomainEvents aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        // Domain handlers must remain in-process and transaction-friendly. External messages
        // should be written to an outbox instead of being published directly from a handler.
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
