namespace SimulationStorm.Collections.Round;

/// <summary>
/// Represents a read-only variant of a collection with limited capacity that automatically removes oldest elements when capacity is exceeded.
/// </summary>
public interface IReadOnlyRoundCollection
{
    /// <summary>
    /// Gets the collection maximum item count.
    /// </summary>
    int Capacity { get; }
}