using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Logging;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace SimulationStorm.Avalonia.MarkupExtensions;

public abstract class BindingExtensionBase : ReflectionBindingExtension
{
    #region Constructors
    protected BindingExtensionBase() { }

    protected BindingExtensionBase(string path) : base(path) { }
    #endregion

    public new IBinding ProvideValue(IServiceProvider serviceProvider) => ProvideValueCore(serviceProvider);

    #region Protected methods
    protected abstract IBinding ProvideValueCore(IServiceProvider serviceProvider);
    
    protected Binding BuildBinding(IServiceProvider serviceProvider) => base.ProvideValue(serviceProvider);
    
    protected static object? TransformInitialBindingValue(object? value)
    {
        if (value is not BindingNotification bindingNotification)
            return value;

        return bindingNotification.HasValue ? bindingNotification.Value : null;
    }

    protected static bool IsBindingValueNotEmpty(object? value) =>
        value is not null
        && value != AvaloniaProperty.UnsetValue
        && value != BindingOperations.DoNothing;
    
    // [NOTE] For now, logging is disabled
    // protected static object? TransformInitialBindingValue(object? value,
    //     AvaloniaObject targetObject, AvaloniaProperty? targetProperty, string bindingPath)
    // {
    //     if (value is not BindingNotification bindingNotification)
    //         return value;
    //
    //     if (bindingNotification.Error is { } error)
    //         LogBindingErrorIfLoggerAvailable(targetObject, targetProperty, bindingPath, error);
    //             
    //     return bindingNotification.HasValue ? bindingNotification.Value : null;
    // }

    // protected static void LogBindingErrorIfLoggerAvailable(AvaloniaObject targetObject,
    //     AvaloniaProperty? targetProperty, string bindingPath, Exception error)
    // {
    //     if (!Logger.TryGet(LogEventLevel.Warning, LogArea.Binding, out var logger))
    //         return;
    //     
    //     logger.Log(targetObject, "An error occurred binding {Property} to {Expression}: {Message}",
    //         (object?)targetProperty ?? "(unknown)", bindingPath, error);
    // }
    #endregion
}