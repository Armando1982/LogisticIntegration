namespace LogisticIntegration.Domain.Weighing;

/// <summary>
/// Represents a single weight measurement reading.
/// This entity encapsulates the weight type, value, and timestamp of a measurement.
/// </summary>
public class WeightReading
{
    public Guid Id { get; private set; }
    public WeightType Type { get; private set; }
    public decimal ValueInKg { get; private set; }
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Creates a new WeightReading instance.
    /// </summary>
    /// <param name="id">Unique identifier for the weight reading</param>
    /// <param name="type">The type of weight measurement (Gross or Tare)</param>
    /// <param name="valueInKg">The weight value in kilograms</param>
    /// <param name="timestamp">The date and time when the measurement was taken</param>
    /// <exception cref="ArgumentException">Thrown when id is empty</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when valueInKg is not greater than 0</exception>
    public WeightReading(Guid id, WeightType type, decimal valueInKg, DateTime timestamp)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (valueInKg <= 0)
            throw new ArgumentOutOfRangeException(nameof(valueInKg), "Weight value must be greater than 0");

        Id = id;
        Type = type;
        ValueInKg = valueInKg;
        Timestamp = timestamp;
    }
}
