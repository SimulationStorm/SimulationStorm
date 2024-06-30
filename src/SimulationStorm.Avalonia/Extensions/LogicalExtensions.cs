using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.LogicalTree;

namespace SimulationStorm.Avalonia.Extensions;

public static class LogicalExtensions
{
    public static void WithDisposables(this ILogical logical, Action<CompositeDisposable> action)
    {
        var disposables = new CompositeDisposable();
        
        logical.DetachedFromLogicalTree += (_, _) => disposables.Dispose();
        
        action(disposables);
    }

    public static IEnumerable<ILogical> GetLogicalSiblingsExceptSelf(this ILogical logical) =>
        logical.GetLogicalSiblings().Where(sibling => sibling.Equals(logical) is false);
    
    public static T? FindNamedLogicalDescendant<T>(this ILogical logical, string name) where T : class, INamed =>
        logical.GetLogicalDescendants().OfType<T>().FirstOrDefault(descendant => descendant.Name == name);
}