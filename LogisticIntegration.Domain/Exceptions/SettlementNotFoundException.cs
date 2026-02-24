namespace LogisticIntegration.Domain.Exceptions
{
    public class SettlementNotFoundException : Exception
    {
        public SettlementNotFoundException(Guid settlementId)
            : base($"Settlement with ID '{settlementId}' was not found.")
        {
            SettlementId = settlementId;
        }

        public Guid SettlementId { get; }
    }
}
