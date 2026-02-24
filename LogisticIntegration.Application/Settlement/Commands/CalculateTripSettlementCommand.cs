using MediatR;

namespace LogisticIntegration.Application.Settlement.Commands
{
    public record CalculateTripSettlementCommand(Guid TripSettlementId) : IRequest<bool>;
}
