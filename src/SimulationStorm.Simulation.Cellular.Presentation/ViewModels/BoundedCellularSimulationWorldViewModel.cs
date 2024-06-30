using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Primitives;
using SimulationStorm.Primitives.Extensions;
using SimulationStorm.Simulation.Cellular.Presentation.Models;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.Presentation;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.Simulation.Cellular.Presentation.ViewModels;

public partial class BoundedCellularSimulationWorldViewModel
(
    IImmediateUiThreadScheduler uiThreadScheduler,
    IWorldViewport worldViewport,
    IWorldCamera worldCamera,
    IBoundedCellularWorldRenderer worldRenderer
)
    : WorldViewModel(uiThreadScheduler, worldViewport, worldCamera, worldRenderer)
{
    public float CameraZoom => WorldCamera.Zoom;
    
    #region Commands
    [RelayCommand]
    private void HighlightCell((Point Cell, CellularWorldPointedCellState State) args) =>
        worldRenderer.PointedCell = (args.Cell, args.State);

    [RelayCommand]
    private void UnhighlightCell()
    {
        if (worldRenderer.PointedCell is not null)
            worldRenderer.PointedCell = null;
    }
    #endregion
    
    public bool TryGetCellUnderPoint(Point absolutePoint, [NotNullWhen(true)] out Point? cell)
    {
        if (WorldCamera.Zoom < 1)
        {
            cell = null;
            return false;
        }

        var relativePoint = AbsoluteToRelativePoint(absolutePoint);
        var simulationImageRect = worldRenderer.SimulationImageRect;
        if (simulationImageRect.Contains(relativePoint))
        {
            cell = relativePoint - simulationImageRect.Position;
            return true;
        }

        cell = null;
        return false;
    }

    #region Private methods
    private Point AbsoluteToRelativePoint(Point absolutePoint)
    {
        Matrix3x2.Invert(WorldCamera.Matrix, out var invertedCameraMatrix);

        var absolutePointVector = absolutePoint.ToVector2();
        var relativePointVector = Vector2.Transform(absolutePointVector, invertedCameraMatrix);

        return relativePointVector.ToPoint();
    }
    #endregion
}