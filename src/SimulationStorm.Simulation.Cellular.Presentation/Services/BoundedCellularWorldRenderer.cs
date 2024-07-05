using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DotNext.Threading;
using SimulationStorm.Graphics;
using SimulationStorm.Presentation.Colors;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Models;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Utilities.Benchmarking;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public class BoundedCellularWorldRenderer : WorldRendererBase, IBoundedCellularWorldRenderer
{
    #region Properties
    public Color DefaultBackgroundColor => _uiColorProvider.BackgroundColor;
    
    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (value == _backgroundColor)
                return;
            
            _backgroundColor = value;
            OnPropertyChanged();
            
            RequestRerender();
        }
    }

    public Rect SimulationImageRect => GetSimulationImageRect();

    public bool IsGridLinesVisible
    {
        get => _isGridLinesVisible;
        set
        {
            if (value == _isGridLinesVisible)
                return;
            
            _isGridLinesVisible = value;
            OnPropertyChanged();
            
            RequestRerender();
        }
    }

    public Color GridLinesColor
    {
        get => _gridLinesPaint.Color;
        set
        {
            if (value == _gridLinesPaint.Color)
                return;
            
            _gridLinesPaint.Color = value;
            OnPropertyChanged();

            RequestRerender();
        }
    }

    public Color HoveredCellColor
    {
        get => _hoveredCellPaint.Color;
        set
        {
            if (value == _hoveredCellPaint.Color)
                return;
            
            _hoveredCellPaint.Color = value;
            OnPropertyChanged();
            
            RequestRerender();
        }
    }

    public Color PressedCellColor
    {
        get => _pressedCellPaint.Color;
        set
        {
            if (value == _pressedCellPaint.Color)
                return;
            
            _pressedCellPaint.Color = value;
            OnPropertyChanged();
            
            RequestRerender();
        }
    }
    
    public (Point Cell, CellularWorldPointedCellState State)? PointedCell
    {
        get => _pointedCell;
        set
        {
            if (value == _pointedCell)
                return;
            
            _pointedCell = value;
            OnPropertyChanged();
            
            RequestRerender();
        }
    }
    #endregion

    #region Fields
    private readonly ISimulationRenderer _simulationRenderer;

    private IBitmap? _simulationImage;

    private readonly AsyncExclusiveLock _simulationImageLock = new();
    
    private readonly IUiColorProvider _uiColorProvider;
    
    private Color _backgroundColor;

    private readonly IPaint _gridLinesPaint;
    private bool _isGridLinesVisible;

    private readonly IPaint _hoveredCellPaint;
    private readonly IPaint _pressedCellPaint;
    private readonly IPaint _simulationImageBorderPaint;
    
    private (Point Cell, CellularWorldPointedCellState State)? _pointedCell;
    #endregion

    public BoundedCellularWorldRenderer
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarker benchmarker,
        IWorldViewport worldViewport,
        IWorldCamera worldCamera,
        ISimulationRenderer simulationRenderer,
        IUiColorProvider uiColorProvider,
        ICellularWorldRendererOptions options
    )
        : base(graphicsFactory, benchmarker, worldViewport, worldCamera)
    {
        _simulationRenderer = simulationRenderer;
        _uiColorProvider = uiColorProvider;
 
        _backgroundColor = uiColorProvider.BackgroundColor;
        _isGridLinesVisible = options.IsGridLinesVisible;
        
        _gridLinesPaint = GraphicsFactory.CreatePaint();
        _gridLinesPaint.Color = options.GridLinesColor;
        
        _hoveredCellPaint = GraphicsFactory.CreatePaint();
        _hoveredCellPaint.Color = options.HoveredCellColor;
        
        _pressedCellPaint = GraphicsFactory.CreatePaint();
        _pressedCellPaint.Color = options.PressedCellColor;

        _simulationImageBorderPaint = GraphicsFactory.CreatePaint();
        _simulationImageBorderPaint.Color = HoveredCellColor;
        _simulationImageBorderPaint.Style = PaintStyle.Stroke;
        
        Observable
            .FromEventPattern<EventHandler<UiColorChangedEventArgs>, UiColorChangedEventArgs>
            (
                h => _uiColorProvider.BackgroundColorChanged += h,
                h => _uiColorProvider.BackgroundColorChanged -= h
            )
            .Select(e => e.EventArgs)
            .Where(e => BackgroundColor == e.PreviousColor) // Set background color from ui color provider only if it was not changed manually
            .Subscribe(e => BackgroundColor = e.NewColor)
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler<SimulationRenderingCompletedEventArgs>, SimulationRenderingCompletedEventArgs>
            (
                h => _simulationRenderer.RenderingCompleted += h,
                h => _simulationRenderer.RenderingCompleted -= h
            )
            .Subscribe(e => _ = UpdateSimulationImageCopy())
            .DisposeWith(Disposables);
        
        Disposables.AddRange(_gridLinesPaint, _hoveredCellPaint, _pressedCellPaint);
        
        RequestRerender();
    }
    
    protected override async Task RenderAsync(ICanvas canvas)
    {
        canvas.Clear(BackgroundColor);
        canvas.SetMatrix(WorldCamera.Matrix);

        await RenderSimulationImage(canvas)
            .ConfigureAwait(false);
        
        RenderPointedCellIfNeeded(canvas);
        RenderGridLinesIfNeeded(canvas);
        RenderSimulationImageBorder(canvas);
    }

    #region Simulation image rendering
    private async Task UpdateSimulationImageCopy()
    {
        await _simulationImageLock
            .AcquireAsync()
            .ConfigureAwait(false);
        
        _simulationImage?.Dispose();
        _simulationImage = _simulationRenderer.RenderedImage?.Copy();
        
        _simulationImageLock.Release();
        
        if (!IsDisposingOrDisposed)
            RequestRerender();
    }
    
    private async Task RenderSimulationImage(ICanvas canvas)
    {
        await _simulationImageLock
            .AcquireAsync()
            .ConfigureAwait(false);

        if (_simulationImage is not null)
        {
            var position = GetPositionToRenderSimulationImageAt().ToPointF();
            canvas.DrawBitmap(_simulationImage, position);
        }
        
        _simulationImageLock.Release();
    }

    private Point GetPositionToRenderSimulationImageAt() =>
        GetSimulationImageRect().Position;

    private Rect GetSimulationImageRect() =>
        CenterChildSizeRelativeToParentSize(WorldViewport.Size, _simulationRenderer.RenderedImage?.Size ?? Size.Zero);
    #endregion

    #region Pointed cell rendering
    private bool IsPointedCellRenderingNeeded() => WorldCamera.Zoom >= 1 && PointedCell is not null;
    
    private void RenderPointedCellIfNeeded(ICanvas canvas)
    {
        if (!IsPointedCellRenderingNeeded())
            return;

        var pointedCell = PointedCell!.Value;
        
        var simulationImagePosition = GetPositionToRenderSimulationImageAt();
        var cellAbsolutePosition = pointedCell.Cell + simulationImagePosition;

        var paint = pointedCell.State is CellularWorldPointedCellState.Hovered ? _hoveredCellPaint : _pressedCellPaint;
        canvas.DrawRect(cellAbsolutePosition.X, cellAbsolutePosition.Y, 1, 1, paint);
    }
    #endregion
    
    #region Grid lines rendering
    private bool IsGridLinesRenderingNeeded() => WorldCamera.Zoom >= 2 && IsGridLinesVisible;
    
    private void RenderGridLinesIfNeeded(ICanvas canvas)
    {
        if (!IsGridLinesRenderingNeeded())
            return;
            
        var (startX, startY) = GetPositionToRenderSimulationImageAt();
        
        int endX = startX + _simulationRenderer.RenderedImage!.Size.Width,
            endY = startY + _simulationRenderer.RenderedImage!.Size.Height;

        var gridLines = new List<LineF>();
        
        // Vertical lines
        for (var x = startX; x <= endX; x++)
            gridLines.Add(new LineF(x, startY, x, endY));
        
        // Horizontal lines
        for (var y = startY; y <= endY; y++)
            gridLines.Add(new LineF(startX, y, endX, y));
        
        canvas.DrawLines(gridLines, _gridLinesPaint);
    }
    #endregion

    private void RenderSimulationImageBorder(ICanvas canvas)
    {
        var simulationImageRect = GetSimulationImageRect();
        var borderRect = new RectF(simulationImageRect.Left, simulationImageRect.Top,
            simulationImageRect.Right + 1, simulationImageRect.Bottom + 1);
        canvas.DrawRect(borderRect, _simulationImageBorderPaint);
    }

    private static Rect CenterChildSizeRelativeToParentSize(Size parentSize, Size childSize)
    {
        var position = new Point
        (
            (parentSize.Width - childSize.Width) / 2,
            (parentSize.Height - childSize.Height) / 2
        );

        var end = new Point
        (
            position.X + childSize.Width - 1,
            position.Y + childSize.Height - 1
        );
        
        return new Rect
        (
            position.X,
            position.Y,
            end.X,
            end.Y
        );
    }
}