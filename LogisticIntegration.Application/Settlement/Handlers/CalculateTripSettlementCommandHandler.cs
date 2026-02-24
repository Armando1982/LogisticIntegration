using MediatR;
using LogisticIntegration.Application.Interfaces;
using LogisticIntegration.Application.Settlement.Commands;
using LogisticIntegration.Domain.Settlement;

namespace LogisticIntegration.Application.Settlement.Handlers
{
    public class CalculateTripSettlementCommandHandler : IRequestHandler<CalculateTripSettlementCommand, bool>
    {
        private readonly ITripSettlementRepository _repository;

        public CalculateTripSettlementCommandHandler(ITripSettlementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(CalculateTripSettlementCommand request, CancellationToken cancellationToken)
        {
            var settlement = await _repository.GetByIdAsync(request.TripSettlementId, cancellationToken);

            if (settlement == null)
            {
                throw new Exception("Settlement not found");
            }

            settlement.CalculateSettlement();

            await _repository.UpdateAsync(settlement, cancellationToken);

            return true;
        }
    }
}
