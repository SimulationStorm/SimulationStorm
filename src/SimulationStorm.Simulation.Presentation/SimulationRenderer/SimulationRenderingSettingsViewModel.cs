using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Primitives;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.SimulationRenderer;

public partial class SimulationRenderingSettingsViewModel : DisposableObservableObject
{
    #region Properties
    public bool IsRenderingEnabled
    {
        get => _simulationRenderer.IsRenderingEnabled;
        set => _simulationRenderer.IsRenderingEnabled = value;
    }

    public int RenderingInterval
    {
        get => _simulationRenderer.RenderingInterval;
        set => _simulationRenderer.RenderingInterval = value;
    }

    public Range<int> RenderingIntervalRange => _options.RenderingIntervalRange;
    #endregion

    [RelayCommand(CanExecute = nameof(CanResetRenderingInterval))]
    private void ResetRenderingInterval() => RenderingInterval = _options.RenderingInterval;
    private bool CanResetRenderingInterval() => RenderingInterval != _options.RenderingInterval;
    
    #region Fields
    private readonly ISimulationRenderer _simulationRenderer;

    private readonly ISimulationRendererOptions _options;
    #endregion

    public SimulationRenderingSettingsViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        ISimulationRenderer simulationRenderer,
        ISimulationRendererOptions options)
    {
        _simulationRenderer = simulationRenderer;
        _options = options;
        
        WithDisposables(disposables =>
        {
            _simulationRenderer
                .WhenValueChanged(x => x.IsRenderingEnabled, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(IsRenderingEnabled)))
                .DisposeWith(disposables);
            
            _simulationRenderer
                .WhenValueChanged(x => x.RenderingInterval, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(RenderingInterval));
                    ResetRenderingIntervalCommand.NotifyCanExecuteChanged();
                })
                .DisposeWith(disposables);
        });
    }
}