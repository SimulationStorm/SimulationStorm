using System.Numerics;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class WorldCameraOptions
{
    public Range<float> ZoomRange { get; init; }
    
    public Range<float> ZoomChangeRange { get; init; }

    public float DefaultZoomChange { get; init; }

    public float DefaultZoom { get; init; }
    
    public Range<PointF> TranslationRange { get; init; }
    
    public Range<float> TranslationChangeRange { get; init; }

    public float DefaultTranslationChange { get; init; }
    
    public Vector2 DefaultTranslation { get; init; }
}