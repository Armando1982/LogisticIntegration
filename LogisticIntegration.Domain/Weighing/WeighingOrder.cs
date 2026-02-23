namespace LogisticIntegration.Domain.Weighing;

/// <summary>
/// Aggregate Root that represents a complete weighing operation for a collection trip.
/// Manages the entire lifecycle of weighing, including gross weight, tare weight,
/// net weight calculation, and hopper discharge tracking.
/// </summary>
public class WeighingOrder
{
    public Guid Id { get; private set; }
    public Guid CollectionTripId { get; private set; }
    public WeighingStatus Status { get; private set; }
    public decimal? NetWeight { get; private set; }

    private readonly List<WeightReading> _weightReadings;
    private readonly List<HopperDischarge> _discharges;

    /// <summary>
    /// Gets a read-only collection of weight readings recorded during this weighing operation.
    /// </summary>
    public IReadOnlyCollection<WeightReading> WeightReadings => _weightReadings.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of hopper discharges performed during this weighing operation.
    /// </summary>
    public IReadOnlyCollection<HopperDischarge> Discharges => _discharges.AsReadOnly();

    /// <summary>
    /// Creates a new WeighingOrder instance.
    /// Initializes the weighing order in the Pending status.
    /// </summary>
    /// <param name="id">Unique identifier for the weighing order</param>
    /// <param name="collectionTripId">Reference to the collection trip this weighing belongs to</param>
    /// <exception cref="ArgumentException">Thrown when id or collectionTripId are empty</exception>
    public WeighingOrder(Guid id, Guid collectionTripId)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (collectionTripId == Guid.Empty)
            throw new ArgumentException("CollectionTripId cannot be empty", nameof(collectionTripId));

        Id = id;
        CollectionTripId = collectionTripId;
        Status = WeighingStatus.Pending;
        NetWeight = null;

        _weightReadings = new List<WeightReading>();
        _discharges = new List<HopperDischarge>();
    }

    /// <summary>
    /// Records the gross weight measurement for this weighing operation.
    /// Transitions the status from Pending to GrossWeightCaptured.
    /// </summary>
    /// <param name="kg">The gross weight in kilograms</param>
    /// <exception cref="InvalidOperationException">Thrown when Status is not Pending</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when kg is not greater than 0</exception>
    public void RecordGrossWeight(decimal kg)
    {
        if (Status != WeighingStatus.Pending)
            throw new InvalidOperationException($"Cannot record gross weight when status is {Status}. Expected status: Pending");

        if (kg <= 0)
            throw new ArgumentOutOfRangeException(nameof(kg), "Gross weight must be greater than 0");

        var weightReading = new WeightReading(
            Guid.NewGuid(),
            WeightType.Gross,
            kg,
            DateTime.UtcNow
        );

        _weightReadings.Add(weightReading);
        Status = WeighingStatus.GrossWeightCaptured;
    }

    /// <summary>
    /// Records the tare weight measurement and calculates the net weight.
    /// Transitions the status from GrossWeightCaptured to TareWeightCaptured.
    /// Validates that tare weight is less than gross weight.
    /// </summary>
    /// <param name="kg">The tare weight in kilograms</param>
    /// <exception cref="InvalidOperationException">Thrown when Status is not GrossWeightCaptured or when tare weight is not less than gross weight</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when kg is not greater than 0</exception>
    public void RecordTareWeight(decimal kg)
    {
        if (Status != WeighingStatus.GrossWeightCaptured)
            throw new InvalidOperationException($"Cannot record tare weight when status is {Status}. Expected status: GrossWeightCaptured");

        if (kg <= 0)
            throw new ArgumentOutOfRangeException(nameof(kg), "Tare weight must be greater than 0");

        var grossWeight = _weightReadings
            .FirstOrDefault(w => w.Type == WeightType.Gross);

        if (grossWeight == null)
            throw new InvalidOperationException("Gross weight must be recorded before tare weight");

        if (kg >= grossWeight.ValueInKg)
            throw new InvalidOperationException("Tare weight must be less than gross weight");

        var weightReading = new WeightReading(
            Guid.NewGuid(),
            WeightType.Tare,
            kg,
            DateTime.UtcNow
        );

        _weightReadings.Add(weightReading);
        NetWeight = grossWeight.ValueInKg - kg;
        Status = WeighingStatus.TareWeightCaptured;
    }

    /// <summary>
    /// Initiates the discharge of material from a hopper.
    /// Can only start a discharge when the status is GrossWeightCaptured.
    /// </summary>
    /// <param name="hopperId">The identifier of the hopper to discharge</param>
    /// <exception cref="InvalidOperationException">Thrown when Status is not GrossWeightCaptured</exception>
    /// <exception cref="ArgumentException">Thrown when hopperId is empty or whitespace</exception>
    public void StartHopperDischarge(string hopperId)
    {
        if (Status != WeighingStatus.GrossWeightCaptured)
            throw new InvalidOperationException($"Cannot start hopper discharge when status is {Status}. Expected status: GrossWeightCaptured");

        if (string.IsNullOrWhiteSpace(hopperId))
            throw new ArgumentException("HopperId cannot be empty or whitespace", nameof(hopperId));

        var discharge = new HopperDischarge(
            Guid.NewGuid(),
            hopperId,
            DateTime.UtcNow
        );

        _discharges.Add(discharge);
    }
}
