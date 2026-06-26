using Commerce.BuildingBlocks.Domain.Results;
using MediatR;

namespace Catalog.Application.Inventory.Commands.CreateStockItem
{
    public sealed record CreateStockItemCommand(Guid ProductId) : IRequest<Result<CreateStockItemResponse>>;

    public sealed record CreateStockItemResponse(Guid ProductId);
}
