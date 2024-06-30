using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

public abstract partial class DrawingSettingsViewModelBase<TCellState> : DisposableObservableObject, IDrawingSettingsViewModel
{
    #region Properties
    public bool IsDrawingModeEnabled
    {
        get => _settings.IsDrawingEnabled;
        set => _settings.IsDrawingEnabled = value;
    }

    public int BrushRadius
    {
        get => _settings.BrushRadius;
        set => _settings.BrushRadius = value;
    }

    public Range<int> BrushRadiusRange { get; }

    public IEnumerable<DrawingShape> BrushShapes { get; } = Enum.GetValues<DrawingShape>();

    public abstract IEnumerable<object> BrushCellStates { get; }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanChangeBrushShape))]
    private void ChangeBrushShape(DrawingShape drawingShape)
    {
        _settings.BrushShape = drawingShape;
        ChangeBrushShapeCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeBrushShape(DrawingShape drawingShape) => drawingShape != _settings.BrushShape;

    [RelayCommand(CanExecute = nameof(CanChangeBrushCellState))]
    private void ChangeBrushCellState(object cellState)
    {
        _settings.BrushCellState = (TCellState)cellState;
        ChangeBrushCellStateCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeBrushCellState(object cellState)
    {
        // [WORKAROUND] this null check is needed because if null will be passed to a command, it will throw an exception...
        if (cellState is null)
            return false;
        
        return _settings.BrushCellState!.Equals((TCellState)cellState) is false;
    }
    #endregion

    private readonly IDrawingSettings<TCellState> _settings;

    protected DrawingSettingsViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        IDrawingSettings<TCellState> settings,
        IDrawingOptions<TCellState> options)
    {
        _settings = settings;

        BrushRadiusRange = options.BrushRadiusRange;

        WithDisposables(disposables =>
        {
            _settings
                .WhenValueChanged(x => x.IsDrawingEnabled, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(IsDrawingModeEnabled)))
                .DisposeWith(disposables);

            _settings
                .WhenValueChanged(x => x.BrushRadius, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(BrushRadius)))
                .DisposeWith(disposables);
            
            _settings
                .WhenValueChanged(x => x.BrushShape, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => ChangeBrushShapeCommand.NotifyCanExecuteChanged())
                .DisposeWith(disposables);

            _settings
                .WhenValueChanged(x => x.BrushCellState, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ => ChangeBrushCellStateCommand.NotifyCanExecuteChanged())
                .DisposeWith(disposables);
        });
    }
}