using System.Linq;
using System.Threading.Tasks;
using SimulationStorm.Graphics;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.GameOfLife.Presentation.Rendering;

public class GameOfLifeRenderer : BoundedCellularSimulationRendererBase
{
    public (Color DeadCellColor, Color AliveCellColor) CellColors
    {
        get => (_deadCellColor, _aliveCellPaint.Color);
        set
        {
            var (deadCellColor, aliveCellColor) = value;
            if (deadCellColor == _deadCellColor && aliveCellColor == _aliveCellPaint.Color)
                return;
            
            _deadCellColor = deadCellColor;
            _aliveCellPaint.Color = aliveCellColor;
            OnPropertyChanged();
            
            if (IsRenderingEnabled)
                RequestRerender();
        }
    }
    
    #region Fields
    private readonly GameOfLifeManager _gameOfLifeManager;
    
    private Color _deadCellColor;
    
    private readonly IPaint _aliveCellPaint;
    #endregion

    public GameOfLifeRenderer
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IIntervalActionExecutor intervalActionExecutor,
        GameOfLifeManager gameOfLifeManager,
        GameOfLifeRendererOptions options
    )
        : base(graphicsFactory, benchmarker, intervalActionExecutor, gameOfLifeManager, options)
    {
        _gameOfLifeManager = gameOfLifeManager;
        
        _deadCellColor = options.DeadCellColor;
        
        _aliveCellPaint = GraphicsFactory.CreatePaint();
        _aliveCellPaint.Color = options.AliveCellColor;
        
        Disposables.Add(_aliveCellPaint);
    }

    protected override async Task RenderAsync(ICanvas canvas)
    {
        canvas.Clear(_deadCellColor);
        
        var aliveCells = await _gameOfLifeManager.GetAliveCellsAsync();
        var aliveCellRectangles = aliveCells.Select(cell => new RectF(cell.X, cell.Y, 1, 1));
        canvas.DrawRects(aliveCellRectangles, _aliveCellPaint);
    }
}