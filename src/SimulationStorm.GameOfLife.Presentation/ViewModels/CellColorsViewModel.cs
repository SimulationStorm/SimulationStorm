using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Graphics;
using SimulationStorm.GameOfLife.Presentation.Rendering;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.GameOfLife.Presentation.ViewModels;
 
public partial class CellColorsViewModel : DisposableObservableObject
{
    #region Properties
    public Color DeadCellColor
    {
        get => _gameOfLifeRenderer.CellColors.DeadCellColor;
        set => _gameOfLifeRenderer.CellColors = (value, _gameOfLifeRenderer.CellColors.AliveCellColor);
    }

    public Color AliveCellColor
    {
        get => _gameOfLifeRenderer.CellColors.AliveCellColor;
        set => _gameOfLifeRenderer.CellColors = (_gameOfLifeRenderer.CellColors.DeadCellColor, value);
    }
    #endregion

    #region Commands

    [RelayCommand]
    private void RandomizeCellColor(GameOfLifeCellState cellState) =>
        _gameOfLifeRenderer.CellColors = cellState is GameOfLifeCellState.Dead ?
            (ColorUtils.GenerateRandomColor(), AliveCellColor) : (DeadCellColor, ColorUtils.GenerateRandomColor());

    [RelayCommand(CanExecute = nameof(CanResetCellColor))]
    private void ResetCellColor(GameOfLifeCellState cellState) =>
        _gameOfLifeRenderer.CellColors = cellState is GameOfLifeCellState.Dead ?
            (_options.DeadCellColor, AliveCellColor) : (DeadCellColor, _options.AliveCellColor);

    private bool CanResetCellColor(GameOfLifeCellState cellState) => cellState switch
    {
        GameOfLifeCellState.Dead when DeadCellColor != _options.DeadCellColor => true,
        GameOfLifeCellState.Alive when AliveCellColor != _options.AliveCellColor => true,
        _ => false
    };
    
    [RelayCommand]
    private void RandomizeCellColors()
    {
        Color newDeadCellColor = ColorUtils.GenerateRandomColor(),
              newAliveCellColor = newDeadCellColor.Inverted();

        _gameOfLifeRenderer.CellColors = (newDeadCellColor, newAliveCellColor);
    }

    [RelayCommand]
    private void SwapCellColors() => _gameOfLifeRenderer.CellColors = (AliveCellColor, DeadCellColor);
    #endregion

    #region Fields
    private readonly GameOfLifeRenderer _gameOfLifeRenderer;

    private readonly GameOfLifeRendererOptions _options;
    #endregion

    public CellColorsViewModel(IUiThreadScheduler uiThreadScheduler, GameOfLifeRenderer gameOfLifeRenderer, GameOfLifeRendererOptions options)
    {
        _gameOfLifeRenderer = gameOfLifeRenderer;
        _options = options;
        
        WithDisposables(disposables =>
        {
            _gameOfLifeRenderer
                .WhenValueChanged(x => x.CellColors, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(DeadCellColor));
                    OnPropertyChanged(nameof(AliveCellColor));
                    
                    ResetCellColorCommand.NotifyCanExecuteChanged();
                })
                .DisposeWith(disposables);
        });
    }
}