using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SimulationStorm.Collections.Extensions;

public static class NotifyCollectionChangedEventArgsExtensions
{
    public static IEnumerable<T> NewItems<T>(this NotifyCollectionChangedEventArgs e) => e.NewItems?.Cast<T>() ?? Enumerable.Empty<T>();

    public static int NewItemCount(this NotifyCollectionChangedEventArgs e) => e.NewItems?.Count ?? 0;
    
    public static IEnumerable<T> OldItems<T>(this NotifyCollectionChangedEventArgs e) => e.OldItems?.Cast<T>() ?? Enumerable.Empty<T>();
    
    public static int OldItemCount(this NotifyCollectionChangedEventArgs e) => e.OldItems?.Count ?? 0;
}