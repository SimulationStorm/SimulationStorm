using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace SimulationStorm.Avalonia.Helpers;

// [WORKAROUND] The problem with ToolTip is in that it does not react to theme variant changes...
public class ToolTipHelper : AvaloniaObject
{
    public static readonly AttachedProperty<IBrush?> BackgroundLightProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("BackgroundLight");

    public static readonly AttachedProperty<IBrush?> BackgroundDarkProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("BackgroundDark");

    public static readonly AttachedProperty<IBrush?> ForegroundLightProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("ForegroundLight");

    public static readonly AttachedProperty<IBrush?> ForegroundDarkProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("ForegroundDark");
    
    public static readonly AttachedProperty<IBrush?> BorderBrushLightProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("BorderBrushLight");

    public static readonly AttachedProperty<IBrush?> BorderBrushDarkProperty =
        AvaloniaProperty.RegisterAttached<ToolTipHelper, ToolTip, IBrush?>("BorderBrushDark");
    
    public static IBrush? GetBackgroundLight(ToolTip toolTip) =>
        toolTip.GetValue(BackgroundLightProperty);
    public static void SetBackgroundLight(ToolTip toolTip, IBrush? brush) =>
        toolTip.SetValue(BackgroundLightProperty, brush);
    
    public static IBrush? GetBackgroundDark(ToolTip toolTip) =>
        toolTip.GetValue(BackgroundDarkProperty);
    public static void SetBackgroundDark(ToolTip toolTip, IBrush? value) =>
        toolTip.SetValue(BackgroundDarkProperty, value);
    
    public static IBrush? GetForegroundLight(ToolTip toolTip) =>
        toolTip.GetValue(ForegroundLightProperty);
    public static void SetForegroundLight(ToolTip toolTip, IBrush? value) =>
        toolTip.SetValue(ForegroundLightProperty, value);
    
    public static IBrush? GetForegroundDark(ToolTip toolTip) =>
        toolTip.GetValue(ForegroundDarkProperty);
    public static void SetForegroundDark(ToolTip toolTip, IBrush? value) =>
        toolTip.SetValue(ForegroundDarkProperty, value);
    
    public static IBrush? GetBorderBrushLight(ToolTip toolTip) =>
        toolTip.GetValue(BorderBrushLightProperty);
    public static void SetBorderBrushLight(ToolTip toolTip, IBrush? value) =>
        toolTip.SetValue(BorderBrushLightProperty, value);
    
    public static IBrush? GetBorderBrushDark(ToolTip toolTip) =>
        toolTip.GetValue(BorderBrushDarkProperty);
    public static void SetBorderBrushDark(ToolTip toolTip, IBrush? value) =>
        toolTip.SetValue(BorderBrushDarkProperty, value);
    
    static ToolTipHelper()
    {
        BackgroundLightProperty.Changed.AddClassHandler<ToolTip>(HandleBackgroundLightChanged);
        BackgroundDarkProperty.Changed.AddClassHandler<ToolTip>(HandleBackgroundDarkChanged);
        ForegroundLightProperty.Changed.AddClassHandler<ToolTip>(HandleForegroundLightChanged);
        ForegroundDarkProperty.Changed.AddClassHandler<ToolTip>(HandleForegroundDarkChanged);
        BorderBrushLightProperty.Changed.AddClassHandler<ToolTip>(HandleBorderBrushLightChanged);
        BorderBrushDarkProperty.Changed.AddClassHandler<ToolTip>(HandleBorderBrushDarkChanged);
    }

    private static void HandleBackgroundLightChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Light)
            toolTip.Background = e.GetNewValue<IBrush?>();
    }
    
    private static void HandleBackgroundDarkChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Dark)
            toolTip.Background = e.GetNewValue<IBrush?>();
    }
    
    private static void HandleForegroundLightChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Light)
            toolTip.Foreground = e.GetNewValue<IBrush?>();
    }
    
    private static void HandleForegroundDarkChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Dark)
            toolTip.Foreground = e.GetNewValue<IBrush?>();
    }
    
    private static void HandleBorderBrushLightChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Light)
            toolTip.BorderBrush = e.GetNewValue<IBrush?>();
    }
    
    private static void HandleBorderBrushDarkChanged(ToolTip toolTip, AvaloniaPropertyChangedEventArgs e)
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Dark)
            toolTip.BorderBrush = e.GetNewValue<IBrush?>();
    }
}