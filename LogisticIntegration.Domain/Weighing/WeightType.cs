namespace LogisticIntegration.Domain.Weighing;

/// <summary>
/// Represents the type of weight measurement in a weighing process.
/// </summary>
public enum WeightType
{
    /// <summary>
    /// Gross weight measurement (total weight including container).
    /// </summary>
    Gross = 0,

    /// <summary>
    /// Tare weight measurement (weight of empty container).
    /// </summary>
    Tare = 1
}
