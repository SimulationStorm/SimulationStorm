using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using SimulationStorm.Dialogs.Presentation;

namespace SimulationStorm.Dialogs.Avalonia;

public class DialogControl : ContentControl
{
    protected override Type StyleKeyOverride => typeof(DialogControl);

    #region Avalonia properties
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DialogControl, string?>(nameof(Title));

    public static readonly StyledProperty<ICommand?> CloseCommandProperty =
        AvaloniaProperty.Register<DialogControl, ICommand?>(nameof(CloseCommand));
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

        if (DataContext is IDialogViewModel dialogViewModel)
            CloseCommand = dialogViewModel.CloseCommand;
    }
}