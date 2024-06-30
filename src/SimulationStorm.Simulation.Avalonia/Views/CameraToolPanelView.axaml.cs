using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Primitives.Avalonia.Extensions;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.ToolPanels.Avalonia;

namespace SimulationStorm.Simulation.Avalonia.Views;

public partial class CameraToolPanelView : ToolPanelControl
{
    public CameraToolPanelView()
    {
        InitializeComponent();
        
        var viewModel = DiContainer.Default.GetRequiredService<CameraSettingsViewModel>();
        DataContext = viewModel;
        
        this.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<RoutedEventArgs>, RoutedEventArgs>
                (
                    h => Joystick.ThumbMoved += h,
                    h => Joystick.ThumbMoved -= h
                )
                .Subscribe(_ =>
                {
                    var thumbOffset = Joystick.ThumbDistanceVector.ToVector2();
                    thumbOffset = - thumbOffset * viewModel.TranslationChange;
                    viewModel.TranslateCommand.Execute(thumbOffset);
                })
                .DisposeWith(disposables);
        });
    }
}