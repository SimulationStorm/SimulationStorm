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
    public event EventHandler? RenderingStarted;
    
    public event EventHandler<RenderingCompletedEventArgs>? RenderingCompleted;
    #endregion

    protected IGraphicsFactory GraphicsFactory { get; }
    
    protected abstract Size SizeToRender { get; }
    
    #region Fields
    private readonly IBenchmarkingService _benchmarkingService;

    private readonly AsyncAutoResetEvent _renderingCycleSynchronizer = new(false);

    private readonly CancellationTokenSource _renderingCycleCts = new();
    
    private readonly Task _renderingCycleTask;
    
    private IBitmap? _bitmap;
    
    private IBitmap? _bitmapCopy;
    
    private ICanvas? _bitmapCanvas;
    #endregion

    protected RendererBase(IGraphicsFactory graphicsFactory, IBenchmarkingService benchmarkingService)
    {
        GraphicsFactory = graphicsFactory;
        _benchmarkingService = benchmarkingService;

        _renderingCycleTask = RenderInCycleAsync(_renderingCycleCts.Token);
    }

    public override async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;
        
        IsDisposed = true;
        
        await _renderingCycleCts
            .CancelAsync()
            .ConfigureAwait(false);
        
        await _renderingCycleTask
            .ConfigureAwait(false);
        
        await _renderingCycleSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
        
        _renderingCycleCts.Dispose();
        
        _bitmap?.Dispose();
        _bitmapCanvas?.Dispose();
        _bitmapCopy?.Dispose();
        
        await base
            .DisposeAsync()
            .ConfigureAwait(false);
        
        GC.SuppressFinalize(this);
    }

    #region Rendering cycle
    public void RequestRerender()
    {
        // Todo: At the moment, do so to avoid exception with object disposed exception
        if (IsDisposed)
            return;
        
        _renderingCycleSynchronizer.Set();
    }

    private async Task RenderInCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                await _renderingCycleSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                await RenderAndNotifyRenderingCompletedAsync()
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException _)
        {
            // Do nothing
        }
    }
    #endregion
    
    #region Rendering methods
    protected abstract Task RenderAsync(ICanvas canvas);

    protected virtual async Task RenderAndNotifyRenderingCompletedAsync()
    {
        if (SizeToRender.Width is 0 || SizeToRender.Height is 0)
            return;
        
        NotifyRenderingStarted();
        
        var benchmarkResult = await _benchmarkingService
            .MeasureAsync(RenderCoreAsync)
            .ConfigureAwait(false);
        
        NotifyRenderingCompleted(benchmarkResult.ElapsedTime);
    }

    protected async Task RenderCoreAsync()
    {
        RecreateBitmapAndCanvasIfSizeChanged();
        
        await RenderAsync(_bitmapCanvas!)
            .ConfigureAwait(false);
        
        _bitmapCanvas!.Flush();

        _bitmapCopy?.Dispose();
        _bitmapCopy = _bitmap!.Copy();
    }
    #endregion

    #region Notifying
    private void NotifyRenderingStarted() =>
        RenderingStarted?.Invoke(this, EventArgs.Empty);
    
    protected virtual void NotifyRenderingCompleted(TimeSpan elapsedTime) =>
        RenderingCompleted?.Invoke(this, new RenderingCompletedEventArgs(elapsedTime));    
    #endregion

    #region Bitmap and canvas creation
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