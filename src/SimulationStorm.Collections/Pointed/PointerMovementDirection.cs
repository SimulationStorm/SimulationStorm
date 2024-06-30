namespace SimulationStorm.Collections.Pointed;

/// <summary>
/// Represents the direction of movement for the pointer within the collection.
/// </summary>
public enum PointerMovementDirection
{
    /// <summary>
    /// Indicates movement towards the end of the collection relative to the previous position.
    /// </summary>
    Forward,
    /// <summary>
    /// Indicates movement towards the beginning of the collection relative to the previous position.
    /// </summary>
    Backward
}