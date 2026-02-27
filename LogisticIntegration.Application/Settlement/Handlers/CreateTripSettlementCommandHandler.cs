using MediatR;
using LogisticIntegration.Application.Interfaces;
using LogisticIntegration.Application.Settlement.Commands;
using LogisticIntegration.Domain.Settlement;

namespace LogisticIntegration.Application.Settlement.Handlers
{
    public class CreateTripSettlementCommandHandler : IRequestHandler<CreateTripSettlementCommand, Guid>
    {
        private readonly ITripSettlementRepository _repository;

        public CreateTripSettlementCommandHandler(ITripSettlementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Guid> Handle(CreateTripSettlementCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var settlement = new TripSettlement(id, request.WeighingOrderId, request.PhysicalNetWeight);

            await _repository.AddAsync(settlement, cancellationToken);

            return id;
        }
    }
}
