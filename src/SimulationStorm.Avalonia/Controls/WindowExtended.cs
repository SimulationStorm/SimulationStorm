using System;
using System.Reactive.Disposables;
using ActiproSoftware.UI.Avalonia.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Avalonia.Helpers;
using TablerIcons;

namespace SimulationStorm.Avalonia.Controls;

[TemplatePart(partRootPanel, typeof(Panel), IsRequired = true)]
public class WindowExtended : Window
{
    private const string partRootPanel = "PART_RootPanel";
    
    protected override Type StyleKeyOverride { get; } = typeof(WindowExtended);

    #region Avalonia properties
    public new static readonly StyledProperty<Icons?> IconProperty =
        AvaloniaProperty.Register<WindowExtended, Icons?>(nameof(Icon));

    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        AvaloniaProperty.Register<WindowExtended, bool>(nameof(IsTitleBarVisible), defaultValue: true);

    public static readonly StyledProperty<bool> IsFullScreenButtonAllowedProperty =
        ChromedTitleBar.IsFullScreenButtonAllowedProperty.AddOwner<WindowExtended>();

    public static readonly StyledProperty<bool> IsMinimizeButtonAllowedProperty =
        ChromedTitleBar.IsMinimizeButtonAllowedProperty.AddOwner<WindowExtended>();
    
    public static readonly StyledProperty<bool> IsMaximizeButtonAllowedProperty =
        ChromedTitleBar.IsMaximizeButtonAllowedProperty.AddOwner<WindowExtended>();

    public static readonly StyledProperty<bool> IsCloseButtonAllowedProperty =
        ChromedTitleBar.IsCloseButtonAllowedProperty.AddOwner<WindowExtended>();
    #endregion

    #region Properties
    public new Icons? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public bool IsFullScreenButtonAllowed
    {
        get => GetValue(IsFullScreenButtonAllowedProperty);
        set => SetValue(IsFullScreenButtonAllowedProperty, value);
    }
    
    public bool IsMinimizeButtonAllowed
    {
        get => GetValue(IsMinimizeButtonAllowedProperty);
        set => SetValue(IsMinimizeButtonAllowedProperty, value);
    }
    
    public bool IsMaximizeButtonAllowed
    {
        get => GetValue(IsMaximizeButtonAllowedProperty);
        set => SetValue(IsMaximizeButtonAllowedProperty, value);
    }
    
    public bool IsCloseButtonAllowed
    {
        get => GetValue(IsCloseButtonAllowedProperty);
        set => SetValue(IsCloseButtonAllowedProperty, value);
    }
    #endregion

    public WindowExtended()
    {
        WindowHelper.SetIconLayoutable(this, new TablerIcon
        {
            [!TablerIcon.IconProperty] = this[!IconProperty],
            Width = 48,
            Height = 48
        });
        
        // [WORKAROUND] These things are needed to hide the default title bar.
        // [NOTE] Why do we need to create a fake window? In order to avoid problems when manually scaling the window.
        SystemDecorations = SystemDecorations.BorderOnly;

        if (!OperatingSystem.IsWindows())
            return;
        
        // On Linux (not tested on Mac) the following code makes no sense.
        this.WithDisposables(disposables =>
        {
            Panel? rootPanel = null;
            
            TemplateAppliedEvent
                .AddClassHandler<WindowExtended>((_, e) => rootPanel = e.NameScope.Find<Panel>(partRootPanel))
                .DisposeWith(disposables);
            
            LoadedEvent
                .AddClassHandler<WindowExtended>((_, _) =>
                {
                    var fakeTitleBar = new ChromedTitleBarExtended { IsVisible = false };
                    rootPanel!.Children.Add(fakeTitleBar);
                    rootPanel!.Children.Remove(fakeTitleBar);
        
                    SystemDecorations = SystemDecorations.Full;
                })
                .DisposeWith(disposables);
        });
    }
}