using System.Threading.Tasks;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.Bounded.Presentation.Services;

public interface IBoundedSimulationManager : ISimulationManager
{
    Size WorldSize { get; }
    
    Task ChangeWorldSizeAsync(Size newSize);
}