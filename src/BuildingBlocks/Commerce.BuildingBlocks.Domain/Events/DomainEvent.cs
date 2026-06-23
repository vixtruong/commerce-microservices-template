namespace Commerce.BuildingBlocks.Domain.Events
{
    /// <summary>
    /// Base record cho domain event.
    /// </summary>
    public abstract record DomainEvent : IDomainEvent
    {
        protected DomainEvent()
            : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
        }

        protected DomainEvent(
            Guid eventId,
            DateTimeOffset occurredAtUtc)
        {
            if (eventId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Event id must not be empty.",
                    nameof(eventId));
            }

            EventId = eventId;
            OccurredAtUtc = occurredAtUtc;
        }

        public Guid EventId { get; init; }

        public DateTimeOffset OccurredAtUtc { get; init; }
    }
}
