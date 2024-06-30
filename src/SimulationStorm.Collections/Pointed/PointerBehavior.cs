using System;

namespace SimulationStorm.Collections.Pointed;

/// <summary>
/// Specifies options for controlling the behavior of a pointer within a <see cref="IPointedCollection"/>.
/// </summary>
[Flags]
public enum PointerBehavior
{
    /// <summary>
    /// No special pointer behavior is applied.
    /// </summary>
    None = 0,
    /// <summary>
    /// Before adding or inserting items to the collection, remove items after the pointer position if there are.
    /// </summary>
    RemoveItemsAheadOfPointerBeforeAddingOrInsertingItems = 1 << 0,
    /// <summary>
    /// After adding or inserting items to the collection, move the pointer to the position of the last item.
    /// </summary>
    MoveToEndAfterAddingOrInsertingItems = 1 << 1
}