using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.Simulation.Cellular.Presentation.Services;

public abstract class BoundedCellularSimulationRendererBase
(
    IGraphicsFactory graphicsFactory,
    IBenchmarkingService benchmarkingService,
    IIntervalActionExecutor intervalActionExecutor,
    IBoundedSimulationManager simulationManager,
    ISimulationRendererOptions options
)
    : SimulationRendererBase(graphicsFactory, benchmarkingService, intervalActionExecutor, simulationManager, options)
{
    protected override Size SizeToRender => simulationManager.WorldSize;
}