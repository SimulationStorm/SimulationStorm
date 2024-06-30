using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SimulationStorm.Avalonia.Helpers;

public partial class TextBoxHelper
{
    public static readonly AttachedProperty<bool> IsCuttingDisabledProperty =
        AvaloniaProperty.RegisterAttached<TextBoxHelper, TextBox, bool>("IsCuttingDisabled");

    public static bool GetIsCuttingDisabled(TextBox textBox) =>
        textBox.GetValue(IsCuttingDisabledProperty);
    
    public static void SetIsCuttingDisabled(TextBox textBox, bool isCuttingDisabled) =>
        textBox.SetValue(IsCuttingDisabledProperty, isCuttingDisabled);

    private static void HandleIsCuttingDisabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        var isCuttingEnabled = e.GetNewValue<bool>();

        if (isCuttingEnabled)
            textBox.CuttingToClipboard += HandleTextBoxCuttingToClipboard;
        else
            textBox.CuttingToClipboard -= HandleTextBoxCuttingToClipboard;
    }
    
    private static void HandleTextBoxCuttingToClipboard(object? _, RoutedEventArgs e) => e.Handled = true;
}