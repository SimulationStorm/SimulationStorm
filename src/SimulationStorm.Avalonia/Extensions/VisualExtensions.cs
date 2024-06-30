using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.VisualTree;

namespace SimulationStorm.Avalonia.Extensions;

public static class VisualExtensions
{
    public static bool TryGetVisualParent(this Visual visual, [NotNullWhen(true)] out Visual? parent)
    {
        parent = visual.GetVisualParent();
        return parent is not null;
    }
    
    public static bool TryGetVisualParent<T>(this Visual visual, [NotNullWhen(true)] out T? parent) where T : class
    {
        parent = visual.GetVisualParent<T>();
        return parent is not null;
    }
}