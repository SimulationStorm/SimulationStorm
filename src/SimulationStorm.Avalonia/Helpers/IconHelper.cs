using Avalonia;
using Avalonia.Controls;
using TablerIcons;

namespace SimulationStorm.Avalonia.Helpers;

public class IconHelper : AvaloniaObject
{
    private const string classHasIcon = "has-icon";
    
    public static readonly AttachedProperty<Icons> IconProperty =
        AvaloniaProperty.RegisterAttached<IconHelper, ContentControl, Icons>("Icon");

    static IconHelper() =>
        IconProperty.Changed.AddClassHandler<ContentControl>(HandleIconChanged);

    public static Icons GetIcon(ContentControl contentControl) =>
        contentControl.GetValue(IconProperty);
    
    public static void SetIcon(ContentControl contentControl, Icons icon) =>
        contentControl.SetValue(IconProperty, icon);

    private static void HandleIconChanged(ContentControl contentControl, AvaloniaPropertyChangedEventArgs e) =>
        contentControl.Classes.Set(classHasIcon, e.NewValue is not null);
}