using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace SimulationStorm.Avalonia.Controls;

public class ProgressControl : ContentControl
{
    public static readonly StyledProperty<IBrush?> ProgressForegroundProperty =
        AvaloniaProperty.Register<ProgressControl, IBrush?>(nameof(ProgressForeground));

    public IBrush? ProgressForeground
    {
        get => GetValue(ProgressForegroundProperty);
        set => SetValue(ProgressForegroundProperty, value);
    }
}