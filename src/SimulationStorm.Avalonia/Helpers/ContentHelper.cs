using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Helpers;

public class ContentHelper : AvaloniaObject
{
    private const string classHasDisabledContent = "has-disabled-content";
    
    public static readonly AttachedProperty<object?> DisabledContentProperty =
        AvaloniaProperty.RegisterAttached<ContentHelper, ContentControl, object?>("DisabledContent");

    static ContentHelper() =>
        DisabledContentProperty.Changed.AddClassHandler<ContentControl>(HandleDisabledContentChanged);

    public static object? GetDisabledContent(ContentControl contentControl) =>
        contentControl.GetValue(DisabledContentProperty);

    public static void SetDisabledContent(ContentControl contentControl, object? disabledContent) =>
        contentControl.SetValue(DisabledContentProperty, disabledContent);

    private static void HandleDisabledContentChanged(ContentControl contentControl, AvaloniaPropertyChangedEventArgs e) =>
        contentControl.Classes.Set(classHasDisabledContent, e.NewValue is not null);
}