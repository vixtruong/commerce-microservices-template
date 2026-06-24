using MediatR;

namespace Commerce.BuildingBlocks.Domain.Events
{
    /// <summary>
    /// Đại diện cho một sự kiện đã xảy ra bên trong domain.
    ///
    /// INotification cho phép MediatR dispatch event đến một hoặc
    /// nhiều INotificationHandler tương ứng.
    /// </summary>
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Mã định danh duy nhất của event.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Thời điểm event xảy ra theo UTC.
        /// </summary>
        DateTimeOffset OccurredAtUtc { get; }
    }
}
