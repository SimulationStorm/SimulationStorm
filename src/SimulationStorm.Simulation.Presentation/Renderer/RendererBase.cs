using System;
using System.Threading;
using System.Threading.Tasks;
using DotNext.Threading;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Utilities.Benchmarking;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public abstract class RendererBase : AsyncDisposableObservableObject, IRenderer
{
    public IBitmap? RenderedImage => _bitmapCopy;

    #region Events
    public event EventHandler? RenderingStarting;
    
    public event EventHandler<RenderingCompletedEventArgs>? RenderingCompleted;
    #endregion

    protected IGraphicsFactory GraphicsFactory { get; }
    
    protected abstract Size SizeToRender { get; }
    
    #region Fields
    private readonly IBenchmarker _benchmarker;

    private readonly AsyncAutoResetEvent _renderingLoopSynchronizer = new(false);

    private readonly CancellationTokenSource _renderingLoopCts = new();
    
    private Task _renderingLoopTask = null!;
    
    private IBitmap? _bitmap;
    
    private IBitmap? _bitmapCopy;
    
    private ICanvas? _bitmapCanvas;
    #endregion

    protected RendererBase(IGraphicsFactory graphicsFactory, IBenchmarker benchmarker)
    {
        _benchmarker = benchmarker;
        
        GraphicsFactory = graphicsFactory;

        StartRenderingLoop();
    }

    public void RequestRerender()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        _renderingLoopSynchronizer.Set();
    }
    
    protected override async ValueTask DisposeAsyncCore()
    {
        await _renderingLoopCts
            .CancelAsync()
            .ConfigureAwait(false);
        
        await _renderingLoopTask
            .ConfigureAwait(false);
        
        await _renderingLoopSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
        
        _renderingLoopCts.Dispose();
        
        _bitmap?.Dispose();
        _bitmapCanvas?.Dispose();
        _bitmapCopy?.Dispose();
    }
    
    #region Rendering methods
    protected abstract Task RenderAsync(ICanvas canvas);

    protected virtual async Task RenderAndNotifyStartingAndCompletedAsync()
    {
        if (SizeToRender.Width is 0 || SizeToRender.Height is 0)
            return;
        
        NotifyRenderingStarting();
        
        var benchmarkResult = await _benchmarker
            .MeasureAsync(RenderAsyncCore)
            .ConfigureAwait(false);
        
        NotifyRenderingCompleted(benchmarkResult.ElapsedTime);
    }

    protected async Task RenderAsyncCore()
    {
        RecreateBitmapAndCanvasIfSizeChanged();
        
        await RenderAsync(_bitmapCanvas!)
            .ConfigureAwait(false);
        
        _bitmapCanvas!.Flush();

        _bitmapCopy?.Dispose();
        _bitmapCopy = _bitmap!.Copy();
    }
    #endregion

    #region Rendering loop methods
    private void StartRenderingLoop() => 
        _renderingLoopTask = RenderInLoopAsync(_renderingLoopCts.Token);

    private async Task RenderInLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                await _renderingLoopSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                await RenderAndNotifyStartingAndCompletedAsync()
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException _) { }
    }
    #endregion

    #region Notification methods
    protected void NotifyRenderingStarting() =>
        RenderingStarting?.Invoke(this, EventArgs.Empty);
    
    protected virtual void NotifyRenderingCompleted(TimeSpan elapsedTime) =>
        RenderingCompleted?.Invoke(this, new RenderingCompletedEventArgs(elapsedTime));    
    #endregion

    #region Bitmap and canvas creation methods
    private void RecreateBitmapAndCanvasIfSizeChanged()
    {
        if (_bitmap?.Size == SizeToRender)
            return;
        
        _bitmap?.Dispose();
        _bitmapCanvas?.Dispose();
        
        CreateBitmapAndCanvas();
    }

    private void CreateBitmapAndCanvas()
    {
        _bitmap = GraphicsFactory.CreateBitmap(SizeToRender);
        _bitmapCanvas = GraphicsFactory.CreateCanvas(_bitmap);
    }
    #endregion
}