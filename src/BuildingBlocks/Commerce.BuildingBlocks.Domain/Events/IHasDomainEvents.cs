namespace Commerce.BuildingBlocks.Domain.Events
{
    /// <summary>
    /// Cho phép Unit of Work lấy các domain event đang chờ xử lý.
    /// </summary>
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}
