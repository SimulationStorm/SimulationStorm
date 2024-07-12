using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using DotNext.Collections.Generic;
using DynamicData;
using DynamicData.Binding;
using GenericCellularAutomation.Presentation.CellStates;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Rendering;

public class GenericCellularAutomationRenderer : SimulationRendererBase
{
    protected override Size SizeToRender => _genericCellularAutomationManager.WorldSize;

    #region Fields
    private readonly GenericCellularAutomationManager _genericCellularAutomationManager;

    private readonly IDictionary<CellStateModel, IDisposable> _cellStateModelSubscriptions =
        new Dictionary<CellStateModel, IDisposable>();

    private readonly IDictionary<byte, IPaint> _paintByCellStates = new Dictionary<byte, IPaint>();
    #endregion
    
    public GenericCellularAutomationRenderer
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IIntervalActionExecutor intervalActionExecutor,
        GenericCellularAutomationManager genericCellularAutomationManager,
        GenericCellularAutomationSettings settings,
        ISimulationRendererOptions options
    )
        : base(graphicsFactory, benchmarker, intervalActionExecutor, options)
    {
        _genericCellularAutomationManager = genericCellularAutomationManager;
        
        settings.CellStateModels.ForEach(OnCellStateModelAdded);

        settings.CellStateModels
            .ToObservableChangeSet()
            .OnItemAdded(OnCellStateModelAdded)
            .OnItemRemoved(OnCellStateModelRemoved)
            .Subscribe()
            .DisposeWith(Disposables);
    }

    #region Protected methods
    protected override async Task RenderAsync(ICanvas canvas)
    {
        var cellPositionsByStates = await _genericCellularAutomationManager.GetAllCellPositionsByStatesAsync();

        foreach (var (cellState, cellPositions) in cellPositionsByStates)
        {
            var cellPaint = _paintByCellStates[cellState];
            var cellRectangles = cellPositions.Select(cell => new RectF(cell.X, cell.Y, 1, 1));
            canvas.DrawRects(cellRectangles, cellPaint);
        }
    }

    protected override ValueTask DisposeAsyncCore()
    {
        foreach (var (_, subscription) in _cellStateModelSubscriptions)
            subscription.Dispose();
        
        foreach (var (_, cellPaint) in _paintByCellStates)
            cellPaint.Dispose();
        
        return base.DisposeAsyncCore();
    }
    #endregion

    #region Private methods
    private void OnCellStateModelAdded(CellStateModel cellStateModel)
    {
        var subscription = SubscribeOnCellStateModelColorChange(cellStateModel);
        _cellStateModelSubscriptions[cellStateModel] = subscription;
    }

    private void OnCellStateModelRemoved(CellStateModel cellStateModel)
    {
        var subscription = _cellStateModelSubscriptions[cellStateModel];
        _cellStateModelSubscriptions.Remove(cellStateModel);
        subscription.Dispose();

        var paint = _paintByCellStates[cellStateModel.CellState];
        _paintByCellStates.Remove(cellStateModel.CellState);
        paint.Dispose();
    }

    private IDisposable SubscribeOnCellStateModelColorChange(CellStateModel cellStateModel) => cellStateModel
        .WhenValueChanged(x => x.Color)
        .Subscribe(newColor =>
        {
            var cellState = cellStateModel.CellState;

            if (!_paintByCellStates.TryGetValue(cellState, out var cellPaint))
            {
                cellPaint = GraphicsFactory.CreatePaint();
                _paintByCellStates[cellState] = cellPaint;
            }

            cellPaint.Color = newColor;
            
            RequestRerender();
        });
    #endregion
}