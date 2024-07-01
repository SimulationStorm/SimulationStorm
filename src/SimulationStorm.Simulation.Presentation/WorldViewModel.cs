using System;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Renderer;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation;

public partial class WorldViewModel : DisposableObservableObject
{
    #region Properties
    public IBitmap? WorldImage => _worldRenderer.RenderedImage;

    public Size WorldSize
    {
        get => WorldViewport.Size;
        set => WorldViewport.Size = value;
    }
    #endregion

    #region Protected properties
    protected IWorldViewport WorldViewport { get; }
    
    protected IWorldCamera WorldCamera { get; }
    #endregion
    
    #region Commands
    [RelayCommand]
    private void ZoomCameraToViewportCenter(CameraZoomDirection zoomDirection) =>
        WorldCamera.ZoomToViewportCenter(zoomDirection);

    [RelayCommand]
    private void ZoomCameraToPoint((PointF Point, CameraZoomDirection ZoomDirection) args) =>
        WorldCamera.ZoomToPoint(args.Point, args.ZoomDirection);

    [RelayCommand]
    private void TranslateCamera(Vector2 offset) =>
        WorldCamera.Translate(offset);
    
    [RelayCommand]
    private void TranslateCameraByDirection(CameraTranslationDirection translationDirection) =>
        WorldCamera.Translate(translationDirection);
    #endregion

    private readonly IWorldRenderer _worldRenderer;
    
    public WorldViewModel
    (
        IImmediateUiThreadScheduler uiThreadScheduler,
        IWorldViewport worldViewport,
        IWorldCamera worldCamera,
        IWorldRenderer worldRenderer)
    {
        WorldViewport = worldViewport;
        WorldCamera = worldCamera;
        _worldRenderer = worldRenderer;
        
        Observable
            .FromEventPattern<EventHandler<RenderingCompletedEventArgs>, RenderingCompletedEventArgs>
            (
                h => worldRenderer.RenderingCompleted += h,
                h => worldRenderer.RenderingCompleted -= h
            )
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => OnPropertyChanged(nameof(WorldImage)))
            .DisposeWith(Disposables);
    }
}