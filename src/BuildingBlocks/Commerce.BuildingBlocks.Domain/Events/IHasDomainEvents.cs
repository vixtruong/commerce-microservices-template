namespace Commerce.BuildingBlocks.Domain.Events
{
    /// <summary>
    /// Cho phép Unit of Work hoặc DbContext lấy domain event
    /// từ các aggregate đang được theo dõi.
    /// </summary>
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}
