using LogisticIntegration.Domain.Settlement;

namespace LogisticIntegration.Application.Interfaces
{
    public interface ITripSettlementRepository
    {
        Task<TripSettlement?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(TripSettlement settlement, CancellationToken cancellationToken);
        Task AddAsync(TripSettlement settlement, CancellationToken cancellationToken);
    }
}
