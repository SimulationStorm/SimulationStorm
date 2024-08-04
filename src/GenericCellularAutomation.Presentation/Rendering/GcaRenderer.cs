using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using DynamicData.Binding;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Management;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Rendering;

public sealed class GcaRenderer : SimulationRendererBase
{
    protected override Size SizeToRender => _gcaManager.WorldSize;

    #region Fields
    private readonly GcaManager _gcaManager;

    private readonly IDictionary<GcaCellState, IPaint> _paintByCellStates = new Dictionary<GcaCellState, IPaint>();
    #endregion
    
    public GcaRenderer
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IIntervalActionExecutor intervalActionExecutor,
        GcaManager gcaManager,
        GcaSettings settings,
        ISimulationRendererOptions options
    )
        : base(graphicsFactory, benchmarker, intervalActionExecutor, options)
    {
        _gcaManager = gcaManager;
        
        settings
            .WhenValueChanged(x => x.CellStateCollectionDescriptor)
            .Subscribe(UpdatePaintByCellStates!)
            .DisposeWith(Disposables);
    }

    #region Protected methods
    protected override async Task RenderAsync(ICanvas canvas)
    {
        var cellPositionsByStates = await _gcaManager.GetAllCellPositionsByStatesAsync();

        foreach (var (cellState, cellPositions) in cellPositionsByStates)
        {
            var cellPaint = _paintByCellStates[cellState];
            var cellRectangles = cellPositions.Select(cell => new RectF(cell.X, cell.Y, 1, 1));
            canvas.DrawRects(cellRectangles, cellPaint);
        }
    }

    protected override ValueTask DisposeAsyncCore()
    {
        DisposeAndClearCellPaints();
        return base.DisposeAsyncCore();
    }
    #endregion

    #region Private methods
    private void UpdatePaintByCellStates(CellStateCollectionDescriptor cellStateCollectionDescriptor)
    {
        DisposeAndClearCellPaints();
                
        foreach (var cellStateDescriptor in cellStateCollectionDescriptor!.CellStates)
        {
            var cellPaint = GraphicsFactory.CreatePaint();
            cellPaint.Color = cellStateDescriptor.Color;
            
            _paintByCellStates[cellStateDescriptor.CellState] = cellPaint;
        }
    }

    private void DisposeAndClearCellPaints()
    {
        foreach (var (_, cellPaint) in _paintByCellStates)
            cellPaint.Dispose();
        
        _paintByCellStates.Clear();
    }
    #endregion
}