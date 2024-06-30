using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactions.Custom;

namespace SimulationStorm.Simulation.CellularAutomation.Avalonia.Views;

public partial class WarningScreen : UserControl
{
    #region Avalonia properties
    public static readonly StyledProperty<KeyGesture?> QuitKeyProperty =
        AvaloniaProperty.Register<WarningScreen, KeyGesture?>(
            nameof(QuitKey), defaultValue: new KeyGesture(Key.Escape));

    public static readonly StyledProperty<KeyGesture?> ContinueKeyProperty =
        AvaloniaProperty.Register<WarningScreen, KeyGesture?>(
            nameof(ContinueKey), defaultValue: new KeyGesture(Key.Space));
    #endregion

    #region Properties
    public KeyGesture? QuitKey
    {
        get => GetValue(QuitKeyProperty);
        set => SetValue(QuitKeyProperty, value);
    }

    public KeyGesture? ContinueKey
    {
        get => GetValue(ContinueKeyProperty);
        set => SetValue(ContinueKeyProperty, value);
    }
    #endregion

    #region Avalonia events
    /// <summary>
    /// Defines the <see cref="QuitRequested"/> event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> QuitRequestedEvent =
        RoutedEvent.Register<WarningScreen, RoutedEventArgs>(nameof(WarningScreen), RoutingStrategies.Direct);

    /// <summary>
    /// Defines the <see cref="ContinueRequested"/> event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ContinueRequestedEvent =
        RoutedEvent.Register<WarningScreen, RoutedEventArgs>(nameof(WarningScreen), RoutingStrategies.Direct);
    #endregion
    
    #region Events
    /// <summary>
    /// Raised when the user presses the <see cref="QuitKey"/>.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? QuitRequested;

    /// <summary>
    /// Raised when the user presses the <see cref="ContinueKey"/>.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? ContinueRequested;
    #endregion
    
    public WarningScreen() => InitializeComponent();

    #region Event handlers
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        Focus();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        
        if (QuitKey is not null && IsKeyGesturePressed(e, QuitKey))
            NotifyQuitRequested();
        
        if (ContinueKey is not null && IsKeyGesturePressed(e, ContinueKey))
            NotifyContinueRequested();
    }
    #endregion

    #region Private methods
    private static bool IsKeyGesturePressed(KeyEventArgs e, KeyGesture gesture) =>
        e.Key == gesture.Key && e.KeyModifiers == gesture.KeyModifiers;
    
    private void NotifyQuitRequested() =>
        QuitRequested?.Invoke(this, new RoutedEventArgs(QuitRequestedEvent, this));
    
    private void NotifyContinueRequested() =>
        ContinueRequested?.Invoke(this, new RoutedEventArgs(ContinueRequestedEvent, this));
    #endregion
}