using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SimulationStorm.Avalonia.Helpers;

public partial class TextBoxHelper
{
    public static readonly AttachedProperty<bool> IsPastingDisabledProperty =
        AvaloniaProperty.RegisterAttached<TextBoxHelper, TextBox, bool>("IsPastingDisabled");

    public static bool GetIsPastingDisabled(TextBox textBox) =>
        textBox.GetValue(IsPastingDisabledProperty);
    
    public static void SetIsPastingDisabled(TextBox textBox, bool isPastingDisabled) =>
        textBox.SetValue(IsPastingDisabledProperty, isPastingDisabled);

    private static void HandleIsPastingDisabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        var isPastingEnabled = e.GetNewValue<bool>();

        if (isPastingEnabled)
            textBox.PastingFromClipboard += HandleTextBoxPastingFromClipboard;
        else
            textBox.PastingFromClipboard -= HandleTextBoxPastingFromClipboard;
    }
    
    private static void HandleTextBoxPastingFromClipboard(object? _, RoutedEventArgs e) => e.Handled = true;
}