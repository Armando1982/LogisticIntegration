using MediatR;

namespace LogisticIntegration.Application.Settlement.Commands
{
    public record CreateTripSettlementCommand(Guid WeighingOrderId, decimal PhysicalNetWeight) : IRequest<Guid>;
}