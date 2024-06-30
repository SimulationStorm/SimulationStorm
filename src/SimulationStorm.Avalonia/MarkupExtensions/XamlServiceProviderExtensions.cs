using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace SimulationStorm.Avalonia.MarkupExtensions;

public static class XamlServiceProviderExtensions
{
    public static IProvideValueTarget ValidateAndGetProvideValueTarget(this IServiceProvider serviceProvider)
    {
        if (serviceProvider is not IProvideValueTarget provideValueTarget)
            throw new ArgumentException($"Must implement a {nameof(IProvideValueTarget)}.", nameof(serviceProvider));

        return provideValueTarget;
    }
    
    public static AvaloniaObject ValidateAndGetTargetObject(this IProvideValueTarget provideValueTarget)
    {
        if (provideValueTarget.TargetObject is not AvaloniaObject targetObject)
            throw new InvalidOperationException(
                $"{nameof(IProvideValueTarget)}.{nameof(IProvideValueTarget.TargetObject)}" +
                $" must be an {nameof(AvaloniaObject)}.");

        return targetObject;
    }
    
    public static AvaloniaProperty ValidateAndGetTargetProperty(this IProvideValueTarget provideValueTarget)
    {
        if (provideValueTarget.TargetProperty is not AvaloniaProperty targetProperty)
            throw new InvalidOperationException(
                $"{nameof(IProvideValueTarget)}.{nameof(IProvideValueTarget.TargetProperty)}" +
                $" must be an {nameof(AvaloniaProperty)}.");
        
        return targetProperty;
    }
}