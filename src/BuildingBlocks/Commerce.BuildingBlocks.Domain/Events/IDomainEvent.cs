namespace Commerce.BuildingBlocks.Domain.Events
{
    /// <summary>
    /// Đại diện cho một sự kiện đã xảy ra bên trong bounded context.
    /// Đây là event nội bộ, không phải contract gửi qua RabbitMQ.
    /// </summary>
    public interface IDomainEvent
    {
        Guid EventId { get; }

        DateTimeOffset OccurredAtUtc { get; }
    }
}
