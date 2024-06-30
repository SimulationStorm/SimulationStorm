using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace SimulationStorm.Avalonia.Controls;

/// <summary>
/// Extends the functionality of the <see cref="Joystick"/> by adding ability
/// to repeatedly raise <see cref="Joystick.ThumbMoved"/> event when it is pressed and held.
/// </summary>
public class RepeatJoystick : Joystick
{
    protected override Type StyleKeyOverride => typeof(Joystick);

    #region Styled properties
    /// <summary>
    /// Defines the <see cref="Interval"/> property.
    /// </summary>
    public static readonly StyledProperty<int> IntervalProperty =
        RepeatButton.IntervalProperty.AddOwner<RepeatJoystick>();
    
    /// <summary>
    /// Defines the <see cref="Delay"/> property.
    /// </summary>
    public static readonly StyledProperty<int> DelayProperty =
        RepeatButton.DelayProperty.AddOwner<RepeatJoystick>();
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the amount of time, in milliseconds, of repeating thumb move events.
    /// </summary>
    public int Interval
    {
        get => GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the amount of time, in milliseconds, to wait before repeating begins.
    /// </summary>
    public int Delay
    {
        get => GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }
    #endregion
    
    private DispatcherTimer? _repeatTimer;

    #region Event handlers
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsPressedProperty && change.GetNewValue<bool>() == false)
            StopTimer();
    }
    
    protected override void OnBorderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        base.OnBorderPointerPressed(sender, e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            StartTimer();
    }

    protected override void OnBorderPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        base.OnBorderPointerReleased(sender, e);

        if (e.InitialPressMouseButton == MouseButton.Left)
            StopTimer();
    }
    #endregion

    #region Private methods
    private void StartTimer()
    {
        if (_repeatTimer is null)
        {
            _repeatTimer = new DispatcherTimer();
            _repeatTimer.Tick += OnRepeatTimerTick;
        }

        if (_repeatTimer.IsEnabled)
            return;

        _repeatTimer.Interval = TimeSpan.FromMilliseconds(Delay);
        _repeatTimer.Start();
    }

    private void OnRepeatTimerTick(object? sender, EventArgs e)
    {
        var interval = TimeSpan.FromMilliseconds(Interval);
        if (_repeatTimer!.Interval != interval)
            _repeatTimer.Interval = interval;
        
        OnThumbMoved();
    }

    private void StopTimer() => _repeatTimer?.Stop();
    #endregion
}