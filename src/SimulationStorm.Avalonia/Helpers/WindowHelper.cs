using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using SimulationStorm.Avalonia.Extensions;

namespace SimulationStorm.Avalonia.Helpers;

public class WindowHelper : AvaloniaObject
{
    public static readonly AttachedProperty<Layoutable> IconLayoutableProperty =
        AvaloniaProperty.RegisterAttached<WindowHelper, Window, Layoutable>("IconLayoutable");

    static WindowHelper() =>
        IconLayoutableProperty.Changed.AddClassHandler<Window>(HandleIconLayoutableChanged);
    
    public static Layoutable GetIconLayoutable(Window window) =>
        window.GetValue(IconLayoutableProperty);
    
    public static void SetIconLayoutable(Window window, Layoutable layoutable) =>
        window.SetValue(IconLayoutableProperty, layoutable);

    private static void HandleIconLayoutableChanged(Window window, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not Layoutable layoutable)
            return;
        
        window.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<RoutedEventArgs>, RoutedEventArgs>
                (
                    h => window.Loaded += h,
                    h => window.Loaded -= h
                )
                .Subscribe(_ =>
                {
                    var renderedLayoutable = layoutable.RenderToBitmap();
                    var windowIcon = new WindowIcon(renderedLayoutable);
                    window.Icon = windowIcon;
                })
                .DisposeWith(disposables);
        });
    }
}