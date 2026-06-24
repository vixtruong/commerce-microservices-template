using Commerce.BuildingBlocks.Domain.Events;

namespace Commerce.BuildingBlocks.Domain.Entities
{
    /// <summary>
    /// Base class cho aggregate root.
    /// Aggregate chịu trách nhiệm bảo vệ invariant và thu thập domain event.
    /// </summary>
    public abstract class AggregateRoot<TId>
    : Entity<TId>, IHasDomainEvents
    where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TId id)
            : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents =>
            _domainEvents.AsReadOnly();

        /// <summary>
        /// Ghi nhận domain event trong bộ nhớ của aggregate.
        ///
        /// Method này không tự lưu SQL Server và không gửi RabbitMQ.
        /// </summary>
        protected void RaiseDomainEvent(
            IDomainEvent domainEvent)
        {
            ArgumentNullException.ThrowIfNull(domainEvent);

            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
