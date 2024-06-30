using System;
using System.ComponentModel;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Graphics.Extensions;
using SimulationStorm.Primitives;
using SimulationStorm.Primitives.Extensions;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class WorldCamera : DisposableObject, IWorldCamera
{
    #region Properties
    public Matrix3x2 Matrix => _matrix;

    public float Zoom => _matrix.GetScale().X;

    public float ZoomChange { get; set; }

    public Vector2 Translation => GetNormalizedTranslation();

    public float TranslationChange { get; set; }
    #endregion
    
    public event EventHandler<CameraMatrixChangedEventArgs>? MatrixChanged;

    #region Fields
    private readonly IWorldViewport _worldViewport;
    
    private readonly WorldCameraOptions _options;
    
    private Matrix3x2 _matrix = Matrix3x2.Identity;
    #endregion

    public WorldCamera(IWorldViewport worldViewport, WorldCameraOptions options)
    {
        _worldViewport = worldViewport;
        _options = options;
        
        ZoomChange = options.DefaultZoomChange;
        TranslationChange = options.DefaultTranslationChange;
        
        WithDisposables(disposables =>
        {
            var isInitialized = false;
            Observable
                .FromEventPattern<EventHandler<ViewportSizeChangedEventArgs>, ViewportSizeChangedEventArgs>
                (
                    h => worldViewport.SizeChanged += h,
                    h => worldViewport.SizeChanged -= h
                )
                .Subscribe(_ =>
                {
                    if (!isInitialized)
                    {
                        ZoomToViewportCenter(options.DefaultZoom);
                        isInitialized = true;
                        
                        return;
                    }
                    
                    ZoomToViewportCenter(Zoom);
                })
                .DisposeWith(disposables);
        });
    }

    #region Public methods
    #region Zooming methods
    public bool CanZoomToViewportCenter(CameraZoomDirection zoomDirection) =>
        CanZoomToViewportCenter(GetZoomByZoomDirection(zoomDirection));

    public void ZoomToViewportCenter(CameraZoomDirection zoomDirection) =>
        ZoomToViewportCenter(GetZoomByZoomDirection(zoomDirection));

    public bool CanZoomToViewportCenter(float zoom) =>
        CanZoomToPoint(GetViewportCenterPoint(), zoom);

    public void ZoomToViewportCenter(float zoom)
    {
        var previousMatrix = Matrix;
        _matrix = Matrix3x2.Identity;
        ZoomToPointWithoutNotification(GetViewportCenterPoint(), zoom);
        NotifyMatrixChangedIfNeeded(previousMatrix);
    }

    public bool CanZoomToPoint(PointF point, CameraZoomDirection zoomDirection) =>
        CanZoomToPoint(point, GetZoomByZoomDirection(zoomDirection));
    
    public void ZoomToPoint(PointF point, CameraZoomDirection zoomDirection) =>
        ZoomToPoint(point, GetZoomByZoomDirection(zoomDirection));

    public bool CanZoomToPoint(PointF point, float zoom) =>
        zoom > _options.ZoomRange.Minimum && zoom < _options.ZoomRange.Maximum;

    public void ZoomToPoint(PointF point, float zoom)
    {
        var previousMatrix = Matrix;
        ZoomToPointWithoutNotification(point, zoom);
        NotifyMatrixChangedIfNeeded(previousMatrix);
    }

    public bool CanResetZoom() => Math.Abs(Zoom - _options.DefaultZoom) > 0.001f;

    public void ResetZoom() => ZoomToViewportCenter(_options.DefaultZoom);
    #endregion
    
    #region Translation methods
    public void Translate(CameraTranslationDirection translationDirection) =>
        Translate(GetOffsetByTranslationDirection(translationDirection));

    public void Translate(Vector2 offset)
    {
        var previousMatrix = Matrix;
        SetTranslationWithoutNotification(_matrix.GetTranslation() + offset);
        NotifyMatrixChangedIfNeeded(previousMatrix);
    }

    public void SetTranslation(Vector2 translation)
    {
        var previousMatrix = Matrix;
        SetTranslationWithoutNotification(translation + GetDefaultTranslationForCurrentZoom());
        NotifyMatrixChangedIfNeeded(previousMatrix);
    }

    public bool CanResetTranslation() => Translation != _options.DefaultTranslation;

    public void ResetTranslation() => SetTranslation(_options.DefaultTranslation);
    #endregion
    #endregion

    #region Private methods
    private void ZoomToPointWithoutNotification(PointF point, float zoom)
    {
        zoom = Math.Clamp(zoom, _options.ZoomRange.Minimum, _options.ZoomRange.Maximum);
        
        float zoomRatio = zoom / Zoom,
              invertedZoomRatio = 1 - zoomRatio;

        SetTranslationWithoutNotification(new Vector2
        (
            point.X * invertedZoomRatio + _matrix.GetTranslation().X * zoomRatio,
            point.Y * invertedZoomRatio + _matrix.GetTranslation().Y * zoomRatio
        ));
        
        _matrix.SetScale(new Vector2(zoom, zoom));
    }

    private void SetTranslationWithoutNotification(Vector2 translation)
    {
        var translationRange = _options.TranslationRange;
        PointF minimumTranslation = translationRange.Minimum,
               maximumTranslation = translationRange.Maximum;
        
        translation.X = Math.Clamp(translation.X, minimumTranslation.X, maximumTranslation.X);
        translation.Y = Math.Clamp(translation.Y, minimumTranslation.Y, maximumTranslation.Y);
        
        _matrix.SetTranslation(translation);
    }

    private Vector2 GetNormalizedTranslation() => _matrix.GetTranslation() - GetDefaultTranslationForCurrentZoom();

    private Vector2 GetDefaultTranslationForCurrentZoom()
    {
        var viewportCenter = GetViewportCenterPoint().ToVector2();
        
        var sign = Zoom < 1 ? 1 : -1;
        var translation = Zoom < 1 ? (1 - Zoom) * viewportCenter : (Zoom - 1) * viewportCenter;
        return translation * sign;
    }

    private PointF GetViewportCenterPoint() => new
    (
        _worldViewport.Size.Width / 2f,
        _worldViewport.Size.Height / 2f
    );

    private float GetZoomByZoomDirection(CameraZoomDirection zoomDirection) =>
        Zoom + ZoomChange * (zoomDirection is CameraZoomDirection.ZoomIn ? 1 : -1);
    
    private Vector2 GetOffsetByTranslationDirection(CameraTranslationDirection translationDirection) => translationDirection switch
    {
        CameraTranslationDirection.TopLeft => new Vector2(TranslationChange, TranslationChange),
        CameraTranslationDirection.Top => new Vector2(0, TranslationChange),
        CameraTranslationDirection.TopRight => new Vector2(-TranslationChange, TranslationChange),
        CameraTranslationDirection.Right => new Vector2(-TranslationChange, 0),
        CameraTranslationDirection.BottomRight => new Vector2(-TranslationChange, -TranslationChange),
        CameraTranslationDirection.Bottom => new Vector2(0, -TranslationChange),
        CameraTranslationDirection.BottomLeft => new Vector2(TranslationChange, -TranslationChange),
        CameraTranslationDirection.Left => new Vector2(TranslationChange, 0),
        _ => throw new InvalidEnumArgumentException(nameof(translationDirection), (int)translationDirection, typeof(CameraTranslationDirection))
    };

    private void NotifyMatrixChangedIfNeeded(Matrix3x2 previousMatrix)
    {
        if (Matrix != previousMatrix)
            MatrixChanged?.Invoke(this, new CameraMatrixChangedEventArgs(previousMatrix, Matrix));
    }
    #endregion
}