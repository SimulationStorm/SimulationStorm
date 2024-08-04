using GenericCellularAutomation.Presentation.Management;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Threading.Presentation;

namespace GenericCellularAutomation.Presentation.ViewModels;

public sealed class GcaWorldViewModel
(
    IImmediateUiThreadScheduler uiThreadScheduler,
    IWorldViewport worldViewport,
    IWorldCamera worldCamera,
    IBoundedCellularWorldRenderer worldRenderer,
    GcaManager automationManager,
    IDrawingSettings<byte> drawingSettings
)
    : BoundedCellularAutomationWorldViewModel<GcaManager, byte>(
        uiThreadScheduler, worldViewport, worldCamera, worldRenderer, automationManager, drawingSettings);