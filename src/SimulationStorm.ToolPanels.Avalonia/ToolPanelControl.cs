using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.ToolPanels.Presentation;

namespace SimulationStorm.ToolPanels.Avalonia;

[TemplatePart(partContentGrid, typeof(Grid))]
public class ToolPanelControl : ContentControl
{
    private const string partContentGrid = "PART_ContentGrid";
    
    protected override Type StyleKeyOverride => typeof(ToolPanelControl);

    #region Avalonia properties
    public static readonly StyledProperty<ToolPanel?> ToolPanelProperty =
        AvaloniaProperty.Register<ToolPanelControl, ToolPanel?>(nameof(ToolPanel));
    
    public static readonly StyledProperty<ICommand?> CloseCommandProperty =
        AvaloniaProperty.Register<ToolPanelControl, ICommand?>(nameof(CloseCommand));
    
    public static readonly StyledProperty<object?> SettingsContentProperty =
        AvaloniaProperty.Register<ToolPanelControl, object?>(nameof(SettingsContent));

    public static readonly StyledProperty<double> ContentMinWidthProperty =
        AvaloniaProperty.Register<ToolPanelControl, double>(nameof(ContentMinWidth));

    public static readonly StyledProperty<double> SettingsContentMinWidthProperty =
        AvaloniaProperty.Register<ToolPanelControl, double>(nameof(SettingsContentMinWidth));
    
    public static readonly StyledProperty<bool> IsSettingsContentVisibleProperty =
        AvaloniaProperty.Register<ToolPanelControl, bool>(nameof(IsSettingsContentVisible), defaultValue: true);
    #endregion

    #region Properties
    public ToolPanel? ToolPanel
    {
        get => GetValue(ToolPanelProperty);
        set => SetValue(ToolPanelProperty, value);
    }

    public ICommand? CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public object? SettingsContent
    {
        get => GetValue(SettingsContentProperty);
        set => SetValue(SettingsContentProperty, value);
    }
    
    public double ContentMinWidth
    {
        get => GetValue(ContentMinWidthProperty);
        set => SetValue(ContentMinWidthProperty, value);
    }

    public double SettingsContentMinWidth
    {
        get => GetValue(SettingsContentMinWidthProperty);
        set => SetValue(SettingsContentMinWidthProperty, value);
    }

    public bool IsSettingsContentVisible
    {
        get => GetValue(IsSettingsContentVisibleProperty);
        set => SetValue(IsSettingsContentVisibleProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var contentGrid = e.NameScope.Find<Grid>(partContentGrid)!;
        var contentColDef = contentGrid.ColumnDefinitions[0];
        
        var settingsContentColDef = contentGrid.ColumnDefinitions[2];
        
        this.WithDisposables(disposables =>
        {
            contentColDef
                .Bind(ColumnDefinition.MinWidthProperty, this.GetObservable(ContentMinWidthProperty))
                .DisposeWith(disposables);

            this
                .GetObservable(SettingsContentProperty)
                    .Select(_ => Unit.Default)
                .Merge(this.GetObservable(SettingsContentMinWidthProperty)
                    .Select(_ => Unit.Default))
                .Merge(this.GetObservable(IsSettingsContentVisibleProperty)
                    .Select(_ => Unit.Default))
                .Subscribe(_ =>
                {
                    if (SettingsContent is not null && IsSettingsContentVisible)
                    {
                        settingsContentColDef.MinWidth = SettingsContentMinWidth;
                    }
                    else
                    {
                        settingsContentColDef.MinWidth = 0;
                        settingsContentColDef.Width = GridLength.Auto;
                    }
                })
                .DisposeWith(disposables);
        });
    }
}