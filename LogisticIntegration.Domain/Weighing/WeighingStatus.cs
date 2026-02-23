namespace LogisticIntegration.Domain.Weighing;

/// <summary>
/// Represents the status of a weighing order throughout its lifecycle.
/// </summary>
public enum WeighingStatus
{
    /// <summary>
    /// Initial state - weighing process has not started.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Gross weight has been captured.
    /// </summary>
    GrossWeightCaptured = 1,

    /// <summary>
    /// Tare weight has been captured and net weight calculated.
    /// </summary>
    TareWeightCaptured = 2,

    /// <summary>
    /// All material has been discharged from hoppers.
    /// </summary>
    Discharged = 3
}
