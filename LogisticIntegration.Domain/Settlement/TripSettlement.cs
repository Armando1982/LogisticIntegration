namespace LogisticIntegration.Domain.Settlement;

/// <summary>
/// Aggregate Root that represents the settlement of a collection trip.
/// Manages the reconciliation between documentary weight and physical weight,
/// including penalty calculation for weight discrepancies exceeding tolerance.
/// </summary>
public class TripSettlement
{
    /// <summary>
    /// Tolerance percentage for weight discrepancy (10%).
    /// Shortages within this percentage are acceptable; beyond it, penalties apply.
    /// </summary>
    private const decimal TolerancePercentage = 0.10m;

    public Guid Id { get; private set; }
    public Guid WeighingOrderId { get; private set; }
    public decimal PhysicalNetWeight { get; private set; }
    public DriverPenalty? Penalty { get; private set; }

    private readonly List<ProviderLoad> _providerLoads;

    /// <summary>
    /// Gets a read-only collection of provider loads included in this settlement.
    /// </summary>
    public IReadOnlyCollection<ProviderLoad> ProviderLoads => _providerLoads.AsReadOnly();

    /// <summary>
    /// Creates a new TripSettlement instance.
    /// </summary>
    /// <param name="id">Unique identifier for the trip settlement</param>
    /// <param name="weighingOrderId">Reference to the weighing order this settlement belongs to</param>
    /// <param name="physicalNetWeight">The actual net weight measured in kilograms</param>
    /// <exception cref="ArgumentException">Thrown when id or weighingOrderId are empty</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when physicalNetWeight is not greater than 0</exception>
    public TripSettlement(Guid id, Guid weighingOrderId, decimal physicalNetWeight)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (weighingOrderId == Guid.Empty)
            throw new ArgumentException("WeighingOrderId cannot be empty", nameof(weighingOrderId));

        if (physicalNetWeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(physicalNetWeight), "Physical net weight must be greater than 0");

        Id = id;
        WeighingOrderId = weighingOrderId;
        PhysicalNetWeight = physicalNetWeight;
        Penalty = null;

        _providerLoads = new List<ProviderLoad>();
    }

    /// <summary>
    /// Adds a provider load to this settlement.
    /// </summary>
    /// <param name="providerCode">Code identifying the provider/supplier</param>
    /// <param name="productCode">Code identifying the product</param>
    /// <param name="docWeight">The documentary weight in kilograms</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <exception cref="ArgumentException">Thrown when codes are empty/whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when docWeight or unitPrice are not greater than 0</exception>
    public void AddProviderLoad(string providerCode, string productCode, decimal docWeight, decimal unitPrice)
    {
        var providerLoad = new ProviderLoad(
            Guid.NewGuid(),
            providerCode,
            productCode,
            docWeight,
            unitPrice
        );

        _providerLoads.Add(providerLoad);
    }

    /// <summary>
    /// Calculates the settlement, reconciling documentary weight with physical weight
    /// and applying penalties for discrepancies exceeding the 10% tolerance.
    /// 
    /// The settlement process:
    /// 1. Sums total documentary weight from all provider loads
    /// 2. Calculates the difference: PhysicalNetWeight - TotalDocumentaryWeight
    /// 3. Applies the 10% tolerance rule:
    ///    - If the missing weight (shortage) exceeds 10% of total documentary weight,
    ///      a penalty is calculated for the excess shortage amount
    /// 4. The penalty amount = (excess shortage weight) * (maximum unit price from loads)
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when there are no provider loads to settle</exception>
    public void CalculateSettlement()
    {
        if (!_providerLoads.Any())
            throw new InvalidOperationException("Cannot calculate settlement without provider loads");

        // Calculate total documentary weight
        decimal totalDocumentaryWeight = _providerLoads.Sum(pl => pl.DocumentaryWeight);

        // Calculate the difference (shortage if negative)
        decimal difference = PhysicalNetWeight - totalDocumentaryWeight;

        // If physical weight is less than documentary weight, calculate missing weight
        if (difference < 0)
        {
            decimal missingWeight = Math.Abs(difference);
            decimal toleranceAmount = totalDocumentaryWeight * TolerancePercentage;

            // Check if missing weight exceeds the tolerance
            if (missingWeight > toleranceAmount)
            {
                // Calculate excess weight that exceeds tolerance
                decimal excessMissingWeight = missingWeight - toleranceAmount;

                // Find the maximum unit price
                decimal maxUnitPrice = _providerLoads.Max(pl => pl.UnitPrice);

                // Create and assign the penalty
                Penalty = new DriverPenalty(
                    Guid.NewGuid(),
                    excessMissingWeight,
                    maxUnitPrice
                );
            }
            else
            {
                // Missing weight is within tolerance, no penalty
                Penalty = null;
            }
        }
        else
        {
            // Physical weight meets or exceeds documentary weight, no penalty
            Penalty = null;
        }
    }
}
