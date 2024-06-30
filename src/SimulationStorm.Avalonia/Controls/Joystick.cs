using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace SimulationStorm.Avalonia.Controls;

/// <summary>
/// Represents a virtual joystick control.
/// </summary>
[TemplatePart("PART_Thumb", typeof(Control))]
[TemplatePart("PART_Border", typeof(Border))]
[PseudoClasses(pcPressed)]
public class Joystick : TemplatedControl
{
    private const string pcPressed = ":pressed";

    private const string partThumb = "PART_Thumb";
    private const string partBorder = "PART_Border";
    
    #region Avalonia properties
    /// <summary>
    /// Defines the <see cref="IsPressed"/> property.
    /// </summary>
    public static readonly DirectProperty<Joystick, bool> IsPressedProperty =
        Button.IsPressedProperty.AddOwner<Joystick>(o => o.IsPressed);
    
    /// <summary>
    /// Defines the <see cref="ThumbAngle"/> property.
    /// </summary>
    public static readonly DirectProperty<Joystick, double> ThumbAngleProperty =
        AvaloniaProperty.RegisterDirect<Joystick, double>(nameof(ThumbAngle), o => o.ThumbAngle);

    /// <summary>
    /// Defines the <see cref="ThumbDistance"/> property.
    /// </summary>
    public static readonly DirectProperty<Joystick, double> ThumbDistanceProperty =
        AvaloniaProperty.RegisterDirect<Joystick, double>(nameof(ThumbDistance), o => o.ThumbDistance);

    /// <summary>
    /// Defines the <see cref="ThumbDistanceVector"/> property.
    /// </summary>
    public static readonly DirectProperty<Joystick, Vector> ThumbDistanceVectorProperty =
        AvaloniaProperty.RegisterDirect<Joystick, Vector>(nameof(ThumbDistanceVector), o => o.ThumbDistanceVector);

    /// <summary>
    /// Defines the <see cref="ThumbForce"/> property.
    /// </summary>
    public static readonly DirectProperty<Joystick, double> ThumbForceProperty =
        AvaloniaProperty.RegisterDirect<Joystick, double>(nameof(ThumbForce), o => o.ThumbForce);
    #endregion

    #region Properties
    /// <summary>
    /// Gets whether the Joystick is pressed.
    /// </summary>
    public bool IsPressed
    {
        get => _isPressed;
        private set => SetAndRaise(IsPressedProperty, ref _isPressed, value);
    }
    
    /// <summary>
    /// Gets the thumb translation angle in degrees.
    /// </summary>
    public double ThumbAngle
    {
        get => _thumbAngle;
        private set => SetAndRaise(ThumbAngleProperty, ref _thumbAngle, value);
    }

    /// <summary>
    /// Gets the relative thumb distance from the joystick center.
    /// </summary>
    public double ThumbDistance
    {
        get => _thumbDistance;
        private set => SetAndRaise(ThumbDistanceProperty, ref _thumbDistance, value);
    }

    /// <summary>
    /// Gets the relative thumb distance vector from the joystick center.
    /// </summary>
    public Vector ThumbDistanceVector
    {
        get => _thumbDistanceVector;
        private set => SetAndRaise(ThumbDistanceVectorProperty, ref _thumbDistanceVector, value);
    }

    /// <summary>
    /// Gets how many times the pressed pointer position is greater than the joystick radius.
    /// </summary>
    public double ThumbForce
    {
        get => _thumbForce;
        private set => SetAndRaise(ThumbForceProperty, ref _thumbForce, value);
    }
    #endregion
    
    /// <summary>
    /// Defines the <see cref="ThumbMoved"/> event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ThumbMovedEvent =
        RoutedEvent.Register<Joystick, RoutedEventArgs>(nameof(ThumbMoved), RoutingStrategies.Bubble);

    /// <summary>
    /// Raised when the user moves thumb.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? ThumbMoved
    {
        add => AddHandler(ThumbMovedEvent, value);
        remove => RemoveHandler(ThumbMovedEvent, value);
    }
    
    #region Fields
    private bool _isPressed;
    
    private double _thumbAngle,
                   _thumbForce,
                   _thumbDistance;

    private Vector _thumbDistanceVector;
    
    private Border _border = null!;
    
    private Control _thumb = null!;
    
    private readonly TranslateTransform _thumbTranslation = new();
    #endregion

    #region Event handlers
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _border = e.NameScope.Find<Border>(partBorder)!;
        _border.PointerPressed += OnBorderPointerPressed;
        _border.PointerReleased += OnBorderPointerReleased;
        _border.PointerMoved += OnBorderPointerMoved;
        
        _thumb = e.NameScope.Find<Control>(partThumb)!;
        _thumb.RenderTransform = _thumbTranslation;
    }
    
    protected virtual void OnThumbMoved() => RaiseEvent(new RoutedEventArgs(ThumbMovedEvent));
    #endregion

    #region Border event handlers
    protected virtual void OnBorderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        IsPressed = true;
        UpdatePseudoClasses();

        UpdateThumbTranslationAndPropertiesByBorderPointerPosition(e.GetPosition(_border));
    }
    
    protected virtual void OnBorderPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        IsPressed = false;
        UpdatePseudoClasses();
        
        ResetThumbTranslationAndProperties();
    }

    protected virtual void OnBorderPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!IsPressed)
            return;
        
        UpdateThumbTranslationAndPropertiesByBorderPointerPosition(e.GetPosition(_border));
    }
    #endregion

    #region Private methods
    private void UpdateThumbTranslationAndPropertiesByBorderPointerPosition(Point borderPointerPosition)
    {
        var pointerPositionVectorRelativeToBorderCenter = (Vector)(borderPointerPosition - GetBorderCenter());

        var borderRadius = GetBorderRadius();
        
        var thumbTranslation = pointerPositionVectorRelativeToBorderCenter;
        if (thumbTranslation.Length > borderRadius)
            thumbTranslation *= borderRadius / thumbTranslation.Length;
        
        UpdateThumbTranslation(thumbTranslation);
        UpdateThumbAngle(thumbTranslation);
        UpdateThumbDistance(thumbTranslation);
        UpdateThumbDistanceVector(thumbTranslation);
        UpdateThumbForce(pointerPositionVectorRelativeToBorderCenter);
        
        OnThumbMoved();
    }
    
    private void ResetThumbTranslationAndProperties()
    {
        _thumbTranslation.X = 0;
        _thumbTranslation.Y = 0;

        ThumbAngle = 0;
        ThumbDistance = 0;
        ThumbDistanceVector = Vector.Zero;
        ThumbForce = 0;
        
        OnThumbMoved();
    }

    private void UpdateThumbTranslation(Vector thumbTranslation)
    {
        _thumbTranslation.X = thumbTranslation.X;
        _thumbTranslation.Y = thumbTranslation.Y;
    }

    private void UpdateThumbAngle(Vector thumbTranslation)
    {
        var angleInRadians = - Math.Atan2(thumbTranslation.Y, thumbTranslation.X);
        if (angleInRadians < 0)
            angleInRadians += 2 * Math.PI;
        
        ThumbAngle = angleInRadians * (180 / Math.PI);
    }

    private void UpdateThumbDistance(Vector thumbTranslation) =>
        ThumbDistance = thumbTranslation.Length / GetBorderRadius();

    private void UpdateThumbForce(Vector pointerVectorRelativeToBorderCenter) =>
        ThumbForce = pointerVectorRelativeToBorderCenter.Length / GetBorderRadius();

    private void UpdateThumbDistanceVector(Vector thumbTranslation) =>
        ThumbDistanceVector = thumbTranslation / GetBorderRadius();

    private Point GetBorderCenter() => new(_border.Width / 2, _border.Height / 2);
    
    private double GetBorderRadius() => _border.Width / 2;

    private void UpdatePseudoClasses() => PseudoClasses.Set(pcPressed, IsPressed);
    #endregion
}