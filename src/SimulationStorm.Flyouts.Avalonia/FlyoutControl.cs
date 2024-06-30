using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using SimulationStorm.Flyouts.Presentation;

namespace SimulationStorm.Flyouts.Avalonia;

public class FlyoutControl : ContentControl
{
    protected override Type StyleKeyOverride => typeof(FlyoutControl);

    #region Avalonia properties
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<FlyoutControl, string?>(nameof(Title));

    public static readonly StyledProperty<ICommand?> CloseCommandProperty =
        AvaloniaProperty.Register<FlyoutControl, ICommand?>(nameof(CloseCommand));
    #endregion

    #region Properties
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ICommand? CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }
    #endregion

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is IFlyoutViewModel flyoutViewModel)
            CloseCommand = flyoutViewModel.CloseCommand;
    }
}