using Microsoft.EntityFrameworkCore;
using LogisticIntegration.Application.Interfaces;
using LogisticIntegration.Domain.Entities;

namespace LogisticIntegration.Infrastructure.Persistence.Repositories
{
    public class TripSettlementRepository : ITripSettlementRepository
    {
        private readonly ApplicationDbContext _context;

        public TripSettlementRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TripSettlement> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.TripSettlements
                .Include(x => x.ProviderLoads)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(TripSettlement settlement, CancellationToken cancellationToken)
        {
            _context.TripSettlements.Update(settlement);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
