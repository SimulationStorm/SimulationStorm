using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.Simulation.Presentation.WorldRenderer;

public abstract class WorldRendererBase : RendererBase, IWorldRenderer
{
    #region Protected properties
    protected IWorldViewport WorldViewport { get; }

    protected IWorldCamera WorldCamera { get; }

    protected override Size SizeToRender => WorldViewport.Size;
    #endregion
    
    protected WorldRendererBase
    (
        IGraphicsFactory graphicsFactory,
        IBenchmarkingService benchmarkingService,
        IWorldViewport worldViewport,
        IWorldCamera worldCamera
    )
        : base(graphicsFactory, benchmarkingService)
    {
        WorldViewport = worldViewport;
        WorldCamera = worldCamera;
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<ViewportSizeChangedEventArgs>, ViewportSizeChangedEventArgs>
                (
                    h => worldViewport.SizeChanged += h,
                    h => worldViewport.SizeChanged -= h
                )
                .Subscribe(_ => RequestRerender())
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<CameraMatrixChangedEventArgs>, CameraMatrixChangedEventArgs>
                (
                    h => worldCamera.MatrixChanged += h,
                    h => worldCamera.MatrixChanged -= h
                )
                .Subscribe(_ => RequestRerender())
                .DisposeWith(disposables);
        });
    }
}