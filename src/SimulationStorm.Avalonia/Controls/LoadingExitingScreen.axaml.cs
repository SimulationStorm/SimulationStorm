using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Controls;

public partial class LoadingExitingScreen : UserControl
{
    #region Avalonia properties
    public static readonly StyledProperty<object?> LogotypeProperty =
        AvaloniaProperty.Register<LoadingExitingScreen, object?>(nameof(Logotype));

    public static readonly StyledProperty<bool> IsExitingProperty =
        AvaloniaProperty.Register<LoadingExitingScreen, bool>(nameof(IsExiting));

    public static readonly StyledProperty<bool> IsProgressVisibleProperty =
        AvaloniaProperty.Register<LoadingExitingScreen, bool>(nameof(IsProgressVisible));

    public static readonly StyledProperty<TimeSpan> ProgressTransitionDurationProperty =
        AvaloniaProperty.Register<LoadingExitingScreen, TimeSpan>(
            nameof(ProgressTransitionDuration), defaultValue: TimeSpan.FromSeconds(0.125));
    #endregion

    #region Properties
    public object? Logotype
    {
        get => GetValue(LogotypeProperty);
        set => SetValue(LogotypeProperty, value);
    }

    public bool IsExiting
    {
        get => GetValue(IsExitingProperty);
        set => SetValue(IsExitingProperty, value);
    }

    public bool IsProgressVisible
    {
        get => GetValue(IsProgressVisibleProperty);
        set => SetValue(IsProgressVisibleProperty, value);
    }

    public TimeSpan ProgressTransitionDuration
    {
        get => GetValue(ProgressTransitionDurationProperty);
        set => SetValue(ProgressTransitionDurationProperty, value);
    }
    #endregion
    
    public LoadingExitingScreen() => InitializeComponent();

    #region Public methods
    public Task ShowProgressAsync() => SetProgressVisibilityAndWaitForTransitionComplete(true);

    public Task HideProgressAsync() => SetProgressVisibilityAndWaitForTransitionComplete(false);
    #endregion

    private async Task SetProgressVisibilityAndWaitForTransitionComplete(bool isProgressVisible)
    {
        if (isProgressVisible == IsProgressVisible)
            return;
        
        IsProgressVisible = isProgressVisible;
        await Task.Delay(ProgressTransitionDuration);
    }
}
