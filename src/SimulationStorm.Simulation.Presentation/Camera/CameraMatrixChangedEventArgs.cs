using System;
using System.Numerics;

namespace SimulationStorm.Simulation.Presentation.Camera;

public class CameraMatrixChangedEventArgs(Matrix3x2 previousMatrix, Matrix3x2 newMatrix) : EventArgs
{
    public Matrix3x2 PreviousMatrix { get; } = previousMatrix;

    public Matrix3x2 NewMatrix { get; } = newMatrix;
}