using System;
using System.Numerics;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.Camera;

public interface IWorldCamera
{
    #region Properties
    Matrix3x2 Matrix { get; }

    float Zoom { get; }
    
    float ZoomChange { get; set; }
    
    Vector2 Translation { get; }
    
    float TranslationChange { get; set; }
    #endregion

    event EventHandler<CameraMatrixChangedEventArgs>? MatrixChanged;

    #region Zooming methods
    bool CanZoomToViewportCenter(CameraZoomDirection zoomDirection);
    void ZoomToViewportCenter(CameraZoomDirection zoomDirection);

    bool CanZoomToViewportCenter(float zoom);
    void ZoomToViewportCenter(float zoom);

    bool CanZoomToPoint(PointF point, CameraZoomDirection zoomDirection);
    void ZoomToPoint(PointF point, CameraZoomDirection zoomDirection);

    bool CanZoomToPoint(PointF point, float zoom);
    void ZoomToPoint(PointF point, float zoom);

    bool CanResetZoom();
    void ResetZoom();
    #endregion
    
    #region Translation methods
    void Translate(CameraTranslationDirection translationDirection);

    void Translate(Vector2 offset);

    void SetTranslation(Vector2 translation);

    bool CanResetTranslation();
    void ResetTranslation();
    #endregion
}