using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;

namespace SimulationStorm.Avalonia.Extensions;

public static class ResourceNodeExtensions
{
    public static bool TryGetResourceOnUiThread
    (
        this IResourceNode resourceNode,
        object key,
        [NotNullWhen(true)] out object? value)
    {
        object? foundValue = null;
        
        Dispatcher.UIThread.Invoke(() =>
            resourceNode.TryGetResource(key, null, out foundValue));
        
        value = foundValue;
        return value is not null;
    }
    
    public static bool TryGetResourceOnUiThread
    (
        this IResourceNode resourceNode,
        object key,
        ThemeVariant? theme,
        [NotNullWhen(true)] out object? value)
    {
        object? foundValue = null;
        
        Dispatcher.UIThread.Invoke(() =>
            resourceNode.TryGetResource(key, theme, out foundValue));
        
        value = foundValue;
        return value is not null;
    }
}