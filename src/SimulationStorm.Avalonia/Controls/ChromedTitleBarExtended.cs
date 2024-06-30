using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using ActiproSoftware.UI.Avalonia.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using TablerIcons;

namespace SimulationStorm.Avalonia.Controls;

/// <summary>
/// Extends the functionality of the <see cref="ChromedTitleBar"/> by adding extra features.
/// </summary>
public class ChromedTitleBarExtended : ChromedTitleBar
{
    #region Avalonia properties
    /// <summary>
    /// Defines the <see cref="Icon"/> property.
    /// </summary>
    public static readonly StyledProperty<Icons?> IconProperty =
        AvaloniaProperty.Register<ChromedTitleBarExtended, Icons?>(nameof(Icon));

    /// <summary>
    /// Defines the <see cref="Title"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<ChromedTitleBarExtended, object?>(nameof(Title));
    
    /// <summary>
    /// Defines the <see cref="IsCloseButtonAlwaysVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsCloseButtonAlwaysVisibleProperty =
        AvaloniaProperty.Register<ChromedTitleBarExtended, bool>(nameof(IsCloseButtonAlwaysVisible));
    
    /// <summary>
    /// Defines the <see cref="CloseCommand"/> property.
    /// </summary>
    public static readonly StyledProperty<ICommand?> CloseCommandProperty =
        AvaloniaProperty.Register<ChromedTitleBarExtended, ICommand?>(nameof(CloseCommand));

    /// <summary>
    /// Defines the <see cref="DoesBelongToWindow"/> property.
    /// </summary>
    public static readonly DirectProperty<ChromedTitleBarExtended, bool> DoesBelongToWindowProperty =
        AvaloniaProperty.RegisterDirect<ChromedTitleBarExtended, bool>(nameof(DoesBelongToWindow), o => o.DoesBelongToWindow);
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the icon to display in the title bar.
    /// </summary>
    public Icons? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon to display in the title bar.
    /// </summary>
    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    /// <summary>
    /// Gets or sets the command to execute when the close button is clicked.
    /// </summary>
    public ICommand? CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets whether close button is always visible,
    /// despite it isn't allowed, the title bar don't belong to window or close command isn't set.
    /// </summary>
    public bool IsCloseButtonAlwaysVisible
    {
        get => GetValue(IsCloseButtonAlwaysVisibleProperty);
        set => SetValue(IsCloseButtonAlwaysVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the title bar belong to window.
    /// </summary>
    public bool DoesBelongToWindow => TryGetOwnerWindow(out _);
    #endregion

    #region Event handlers
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        var initialIsCloseButtonAllowed = IsCloseButtonAllowed;
        base.OnApplyTemplate(e);
        IsCloseButtonAllowed = initialIsCloseButtonAllowed;

        var mouseTracker = e.NameScope.Find<Panel>("PART_MouseTracker")!;
        mouseTracker.PointerPressed += OnMouseTrackerPointerPressed;
        
        UpdatePseudoClasses();
    }

    /// <summary>
    /// Permits moving window in fullscreen mode.
    /// </summary>
    private void OnMouseTrackerPointerPressed(object? _, PointerPressedEventArgs e)
    {
        if (!TryGetOwnerWindow(out var window))
            return;

        if (window.WindowState is WindowState.FullScreen)
            e.Handled = true;
    }
    
    protected override void OnToggleFullScreen()
    {
        base.OnToggleFullScreen();
        
        UpdatePseudoClasses();
    }

    protected override void OnClose()
    {
        if (CloseCommand is not null)
            CloseCommand.Execute(null);
        else
            base.OnClose();
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Ensures the :fullscreen pseudo class is set when window full screen state toggled.
    /// </summary>
    private void UpdatePseudoClasses()
    {
        if (!TryGetOwnerWindow(out var window))
            return;
        
        PseudoClasses.Set(":fullscreen", window.WindowState is WindowState.FullScreen);
    }

    private bool TryGetOwnerWindow([NotNullWhen(true)] out Window? window)
    {
        window = this.GetVisualRoot() as Window;
        return window is not null;
    }
    #endregion
}