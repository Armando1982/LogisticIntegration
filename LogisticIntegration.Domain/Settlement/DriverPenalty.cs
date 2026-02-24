namespace LogisticIntegration.Domain.Settlement;

/// <summary>
/// Represents a penalty applied to a driver for weight discrepancies beyond tolerance.
/// Calculated based on missing weight and the maximum unit price from provider loads.
/// </summary>
public class DriverPenalty
{
    public Guid Id { get; private set; }
    public decimal MissingWeight { get; private set; }
    public decimal AppliedMaxPrice { get; private set; }
    public decimal TotalPenaltyAmount { get; private set; }

    /// <summary>
    /// Creates a new DriverPenalty instance.
    /// </summary>
    /// <param name="id">Unique identifier for the penalty record</param>
    /// <param name="missingWeight">The weight that exceeds the tolerance threshold in kilograms</param>
    /// <param name="appliedMaxPrice">The maximum unit price used to calculate the penalty</param>
    /// <exception cref="ArgumentException">Thrown when id is empty</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when missingWeight or appliedMaxPrice are not greater than 0</exception>
    public DriverPenalty(Guid id, decimal missingWeight, decimal appliedMaxPrice)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (missingWeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(missingWeight), "Missing weight must be greater than 0");

        if (appliedMaxPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(appliedMaxPrice), "Applied max price must be greater than 0");

        Id = id;
        MissingWeight = missingWeight;
        AppliedMaxPrice = appliedMaxPrice;
        TotalPenaltyAmount = missingWeight * appliedMaxPrice;
    }
}
