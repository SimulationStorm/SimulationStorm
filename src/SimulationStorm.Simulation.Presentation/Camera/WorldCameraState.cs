using System.Numerics;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class WorldCameraState(float zoom, Vector2 translation)
{
    public float Zoom { get; } = zoom;

    public Vector2 Translation { get; } = translation;
}