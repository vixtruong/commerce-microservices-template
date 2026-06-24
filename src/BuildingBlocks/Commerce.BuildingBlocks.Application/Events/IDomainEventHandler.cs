using Commerce.BuildingBlocks.Domain.Events;
using MediatR;

namespace Commerce.BuildingBlocks.Application.Events
{
    /// <summary>
    /// Handler cho domain event nội bộ.
    /// Thực chất kế thừa INotificationHandler của MediatR.
    /// </summary>
    public interface IDomainEventHandler<
        in TDomainEvent>
        : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent;
}
