using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Helpers;

public partial class TextBoxHelper : AvaloniaObject
{
    static TextBoxHelper()
    {
        IsCopyingDisabledProperty.Changed.AddClassHandler<TextBox>(HandleIsCopyingDisabledChanged);
        IsPastingDisabledProperty.Changed.AddClassHandler<TextBox>(HandleIsPastingDisabledChanged);
        IsCuttingDisabledProperty.Changed.AddClassHandler<TextBox>(HandleIsCuttingDisabledChanged);
    }
}