using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.VisualTree;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.ToolPanels.Presentation;

namespace SimulationStorm.ToolPanels.Avalonia;

public partial class ToolPanelManagerView : UserControl
{
    public static readonly StyledProperty<object?> WorkSpaceContentProperty =
        AvaloniaProperty.Register<ToolPanelManagerView, object?>(nameof(WorkSpaceContent));

    public object? WorkSpaceContent
    {
        get => GetValue(WorkSpaceContentProperty);
        set => SetValue(WorkSpaceContentProperty, value);
    }    
    
    public ToolPanelManagerView()
    {
        InitializeComponent();

        var viewModel = DiContainer.Default.GetRequiredService<ToolPanelManagerViewModel>();
        DataContext = viewModel;

        var topLeftToolPanelColDef = RootGrid.ColumnDefinitions[1];
        var topRightToolPanelColDef = RootGrid.ColumnDefinitions[5];

        var bottomToolPanelsContainerRowDef = RootGrid.RowDefinitions[2];
        var bottomLeftToolPanelColDef = BottomToolPanelsContainer.ColumnDefinitions[0];
        var bottomRightToolPanelColDef = BottomToolPanelsContainer.ColumnDefinitions[2];
        
        // Todo: set bottom tool panels row min height based on bottom tool panels.
        double bottomToolPanelsContainerMinHeight = 390,
               previousBottomToolPanelsContainerHeight = bottomToolPanelsContainerMinHeight;
        
        this.WithDisposables(disposables =>
        {
            BindToolPanelViewModelsToContentControls(disposables);
            BindToolPanelWidthLimitsToColumnDefinitions(disposables);

            // Resetting bottom tool panels container row height after hiding both bottom tool panels
            var isNotificationOnInitialValue = true;
            viewModel
                .WhenAnyValue(x => x.BottomLeftToolPanelViewModel, x => x.BottomRightToolPanelViewModel)
                .Where(x => x.Item1 is null && x.Item2 is null)
                .Subscribe(_ =>
                {
                    if (isNotificationOnInitialValue)
                    {
                        isNotificationOnInitialValue = false;
                        return;
                    }
                    
                    previousBottomToolPanelsContainerHeight = bottomToolPanelsContainerRowDef.ActualHeight;
                    bottomToolPanelsContainerRowDef.Height = GridLength.Auto;
                    bottomToolPanelsContainerRowDef.MinHeight = 0;
                })
                .DisposeWith(disposables);
            
            // Restoring bottom tool panels container row height after showing any one of bottom tool panels
            viewModel
                .WhenAnyValue(x => x.BottomLeftToolPanelViewModel, x => x.BottomRightToolPanelViewModel)
                .Where(x => x.Item1 is not null || x.Item2 is not null)
                .Where(_ => bottomToolPanelsContainerRowDef.Height == GridLength.Auto)
                .Subscribe(_ =>
                {
                    bottomToolPanelsContainerRowDef.Height =
                        new GridLength(previousBottomToolPanelsContainerHeight, GridUnitType.Pixel);
                    bottomToolPanelsContainerRowDef.MinHeight = bottomToolPanelsContainerMinHeight;
                })
                .DisposeWith(disposables);

            // Stretch bottom left tool panel to the full width if bottom right tool panel is not opened
            viewModel
                .WhenAnyValue(x => x.BottomLeftToolPanelViewModel, x => x.BottomRightToolPanelViewModel)
                .Where(x => x.Item1 is not null && x.Item2 is null)
                .Subscribe(_ =>
                {
                    bottomRightToolPanelColDef.Width = GridLength.Auto;
                    bottomLeftToolPanelColDef.Width = GridLength.Star;
                })
                .DisposeWith(disposables);
            
            // Stretch bottom right tool panel to the full width if bottom left tool panel is not opened
            viewModel
                .WhenAnyValue(x => x.BottomLeftToolPanelViewModel, x => x.BottomRightToolPanelViewModel)
                .Where(x => x.Item1 is null && x.Item2 is not null)
                .Subscribe(_ =>
                {
                    bottomLeftToolPanelColDef.Width = GridLength.Auto;
                    bottomRightToolPanelColDef.Width = GridLength.Star;
                })
                .DisposeWith(disposables);
            
            // When both bottom tool panels are opened, divide container width equally between them
            viewModel
                .WhenAnyValue(x => x.BottomLeftToolPanelViewModel, x => x.BottomRightToolPanelViewModel)
                .Where(x => x.Item1 is not null && x.Item2 is not null)
                .Subscribe(_ =>
                {
                    bottomLeftToolPanelColDef.Width = GridLength.Star;
                    bottomRightToolPanelColDef.Width = GridLength.Star;
                })
                .DisposeWith(disposables);
        });

        return;
        
        void BindToolPanelViewModelsToContentControls(CompositeDisposable disposables)
        {
            BindToolPanelViewModelToContentControl
                (
                    viewModel.WhenValueChanged(x => x.TopLeftToolPanelViewModel),
                    TopLeftToolPanelContentControl,
                    topLeftToolPanelColDef
                )
                .DisposeWith(disposables);
            
            BindToolPanelViewModelToContentControl
                (
                    viewModel.WhenValueChanged(x => x.TopRightToolPanelViewModel),
                    TopRightToolPanelContentControl,
                    topRightToolPanelColDef
                )
                .DisposeWith(disposables);
            
            BindToolPanelViewModelToContentControl
                (
                    viewModel.WhenValueChanged(x => x.BottomLeftToolPanelViewModel),
                    BottomLeftToolPanelContentControl,
                    bottomLeftToolPanelColDef
                )
                .DisposeWith(disposables);
            
            BindToolPanelViewModelToContentControl
                (
                    viewModel.WhenValueChanged(x => x.BottomRightToolPanelViewModel),
                    BottomRightToolPanelContentControl,
                    bottomRightToolPanelColDef
                )
                .DisposeWith(disposables);
        }

        void BindToolPanelWidthLimitsToColumnDefinitions(CompositeDisposable disposables)
        {
            BindToolPanelWidthLimitsToColumnDefinition(TopLeftToolPanelContentControl, topLeftToolPanelColDef, true)
                .DisposeWith(disposables);
            BindToolPanelWidthLimitsToColumnDefinition(TopRightToolPanelContentControl, topRightToolPanelColDef, true)
                .DisposeWith(disposables);
            BindToolPanelWidthLimitsToColumnDefinition(BottomLeftToolPanelContentControl, bottomLeftToolPanelColDef)
                .DisposeWith(disposables);
            BindToolPanelWidthLimitsToColumnDefinition(BottomRightToolPanelContentControl, bottomRightToolPanelColDef)
                .DisposeWith(disposables);
        }
    }

    private static IDisposable BindToolPanelViewModelToContentControl
    (
        IObservable<object?> viewModelStream,
        ContentControl toolPanelContentControl,
        ColumnDefinition toolPanelColDef)
    {
        return viewModelStream
            .Subscribe(viewModel =>
            {
                // Resetting previous content to avoid view caching
                toolPanelContentControl.Content = null;

                if (viewModel is null)
                {
                    // Resetting column definition width limits
                    toolPanelColDef.MinWidth = 0;
                    toolPanelColDef.MaxWidth = 0;
                }
                else
                    toolPanelContentControl.Content = viewModel;
            });
    }

    // [NOTE] If we make some panels visible by default, then this method should be revised.
    private static IDisposable BindToolPanelWidthLimitsToColumnDefinition
    (
        TransitioningContentControl toolPanelContentControl,
        ColumnDefinition toolPanelColDef,
        bool setColDefWidthToMinWidth = false)
    {
        CompositeDisposable? toolPanelWidthLimitSubscriptions = null;

        return Observable
            .FromEventPattern<EventHandler<TransitionCompletedEventArgs>, TransitionCompletedEventArgs>
            (
                h => toolPanelContentControl.TransitionCompleted += h,
                h => toolPanelContentControl.TransitionCompleted -= h
            )
            .Select(_ => toolPanelContentControl.FindDescendantOfType<ToolPanelControl>())
            .Subscribe(toolPanelControl =>
            {
                toolPanelWidthLimitSubscriptions?.Dispose();

                if (toolPanelControl is null)
                    return;
                
                toolPanelWidthLimitSubscriptions = new CompositeDisposable(2);
                
                BindToolPanelControlWidthLimitToColDef
                (
                    toolPanelControl,
                    Layoutable.MinWidthProperty,
                    ColumnDefinition.MinWidthProperty
                )
                .DisposeWith(toolPanelWidthLimitSubscriptions);
                
                BindToolPanelControlWidthLimitToColDef
                (
                    toolPanelControl,
                    Layoutable.MaxWidthProperty,
                    ColumnDefinition.MaxWidthProperty
                )
                .DisposeWith(toolPanelWidthLimitSubscriptions);
            });
        
        IDisposable BindToolPanelControlWidthLimitToColDef
        (
            ToolPanelControl toolPanelControl,
            StyledProperty<double> toolPanelWidthLimitProperty,
            StyledProperty<double> colDefWidthLimitProperty)
        {
            var skipPropertyChangedNotification = false;
            
            return toolPanelControl
                .GetObservable(toolPanelWidthLimitProperty)
                .Where(widthLimit => widthLimit is not 0)
                .Subscribe(widthLimit =>
                {
                    if (skipPropertyChangedNotification)
                    {
                        skipPropertyChangedNotification = false;
                        return;
                    }
                    
                    toolPanelColDef.SetValue(colDefWidthLimitProperty, widthLimit);

                    skipPropertyChangedNotification = true;
                    toolPanelControl.SetValue(toolPanelWidthLimitProperty, AvaloniaProperty.UnsetValue);
                    
                    if (setColDefWidthToMinWidth)
                        toolPanelColDef.Width = new GridLength(toolPanelColDef.MinWidth);
                });
        }
    }
}