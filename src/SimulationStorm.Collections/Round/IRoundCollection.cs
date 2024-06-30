namespace SimulationStorm.Collections.Round;

/// <summary>
/// Represents a collection with limited capacity that automatically removes oldest elements when capacity is exceeded.
/// </summary>
public interface IRoundCollection : IReadOnlyRoundCollection
{
    /// <summary>
    /// Gets or sets the collection maximum item count.
    /// </summary>
    new int Capacity { get; set; }
}