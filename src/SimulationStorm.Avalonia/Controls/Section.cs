using System;
using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Controls;

public class Section : ContentControl
{
    protected override Type StyleKeyOverride => typeof(Section);

    #region Avalonia properties
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Section, string?>(nameof(Title));

    public static readonly StyledProperty<object?> HeaderRightContentProperty =
        AvaloniaProperty.Register<Section, object?>(nameof(HeaderRightContent));
    #endregion

    #region Properties
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object? HeaderRightContent
    {
        get => GetValue(HeaderRightContentProperty);
        set => SetValue(HeaderRightContentProperty, value);
    }
    #endregion
}