using System.Collections.Specialized;
using System.ComponentModel;

namespace SimulationStorm.Collections;

/// <summary>
/// Represents an observable collection with additional capabilities for controlling the behavior of clearing.
/// </summary>
public interface IExtendedNotifyCollectionChanged : INotifyCollectionChanged, INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets a value indicating whether collection clearing should use the <see cref="NotifyCollectionChangedAction.Remove"/> action, providing removed items,
    /// or the <see cref="NotifyCollectionChangedAction.Reset"/> action, which does not provide removed items.
    /// </summary>
    bool UseRemoveActionOverReset { get; set; }
}