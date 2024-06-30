using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Graphics;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Cellular.Presentation.ViewModels;

public partial class WorldGridLinesViewModel : DisposableObservableObject
{
    #region Properties
    public bool IsGridLinesVisible
    {
        get => _worldRenderer.IsGridLinesVisible;
        set => _worldRenderer.IsGridLinesVisible = value;
    }

    public Color GridLinesColor
    {
        get => _worldRenderer.GridLinesColor;
        set => _worldRenderer.GridLinesColor = value;
    }

    // [ObservableProperty] private bool _areGridLinesRendered;
    #endregion

    #region Commands
    [RelayCommand]
    private void RandomizeGridLinesColor() => GridLinesColor = ColorUtils.GenerateRandomColor();
    
    [RelayCommand(CanExecute = nameof(CanResetGridLinesColor))]
    private void ResetGridLinesColor() => GridLinesColor = _options.GridLinesColor;
    private bool CanResetGridLinesColor() => GridLinesColor != _options.GridLinesColor;
    #endregion
    
    #region Fields
    private readonly ICellularWorldRenderer _worldRenderer;

    private readonly ICellularWorldRendererOptions _options;
    #endregion

    public WorldGridLinesViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        ICellularWorldRenderer worldRenderer,
        ICellularWorldRendererOptions options)
    {
        _worldRenderer = worldRenderer;
        _options = options;
        
        // UpdateAreGridLinesRendered();
        
        WithDisposables(disposables =>
        {
            // Observable
            //     .FromEventPattern<EventHandler<CameraMatrixChangedEventArgs>, CameraMatrixChangedEventArgs>
            //     (
            //         h => worldCamera.MatrixChanged += h,
            //         h => worldCamera.MatrixChanged -= h
            //     )
            //     .Subscribe(_ => UpdateAreGridLinesRendered())
            //     .DisposeWith(disposables);

            _worldRenderer
                .WhenValueChanged(x => x.IsGridLinesVisible, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(IsGridLinesVisible)))
                .DisposeWith(disposables);
            
            _worldRenderer
                .WhenValueChanged(x => x.GridLinesColor, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(GridLinesColor));
                    ResetGridLinesColorCommand.NotifyCanExecuteChanged();
                })
                .DisposeWith(disposables);
        });
    }

    // private void UpdateAreGridLinesRendered() => AreGridLinesRendered = _worldCamera.Zoom >= 2;
}