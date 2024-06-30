using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SimulationStorm.Avalonia.Helpers;

public partial class TextBoxHelper
{
    public static readonly AttachedProperty<bool> IsCopyingDisabledProperty =
        AvaloniaProperty.RegisterAttached<TextBoxHelper, TextBox, bool>("IsCopyingDisabled");

    public static bool GetIsCopyingDisabled(TextBox textBox) =>
        textBox.GetValue(IsCopyingDisabledProperty);
    
    public static void SetIsCopyingDisabled(TextBox textBox, bool isCopyingDisabled) =>
        textBox.SetValue(IsCopyingDisabledProperty, isCopyingDisabled);

    private static void HandleIsCopyingDisabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        var isCopyingEnabled = e.GetNewValue<bool>();

        if (isCopyingEnabled)
            textBox.CopyingToClipboard += HandleTextBoxCopyingToClipboard;
        else
            textBox.CopyingToClipboard -= HandleTextBoxCopyingToClipboard;
    }
    
    private static void HandleTextBoxCopyingToClipboard(object? _, RoutedEventArgs e) => e.Handled = true;
}