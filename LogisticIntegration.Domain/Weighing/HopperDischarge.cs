namespace LogisticIntegration.Domain.Weighing;

/// <summary>
/// Represents the discharge of material from a hopper during a weighing operation.
/// Tracks when discharge started and when it completed.
/// </summary>
public class HopperDischarge
{
    public Guid Id { get; private set; }
    public string HopperId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }

    /// <summary>
    /// Creates a new HopperDischarge instance.
    /// </summary>
    /// <param name="id">Unique identifier for the discharge operation</param>
    /// <param name="hopperId">The identifier of the hopper being discharged</param>
    /// <param name="startTime">The date and time when the discharge started</param>
    /// <exception cref="ArgumentException">Thrown when id is empty or hopperId is empty/whitespace</exception>
    public HopperDischarge(Guid id, string hopperId, DateTime startTime)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(hopperId))
            throw new ArgumentException("HopperId cannot be empty or whitespace", nameof(hopperId));

        Id = id;
        HopperId = hopperId;
        StartTime = startTime;
        EndTime = null;
    }

    /// <summary>
    /// Marks the hopper discharge as complete by recording the end time.
    /// </summary>
    /// <param name="endTime">The date and time when the discharge ended</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when endTime is before the StartTime</exception>
    public void CompleteDischarge(DateTime endTime)
    {
        if (endTime < StartTime)
            throw new ArgumentOutOfRangeException(nameof(endTime), "End time cannot be before start time");

        EndTime = endTime;
    }
}
