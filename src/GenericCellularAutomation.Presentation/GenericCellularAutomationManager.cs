using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation;

public class GenericCellularAutomationManager : SimulationManagerBase
{
    public Size WorldSize { get; }

    private readonly IGenericCellularAutomation<byte> _genericCellularAutomation;
    
    public GenericCellularAutomationManager(IBenchmarker benchmarker) : base(benchmarker)
    {
        
    }

    public Task<IReadOnlyDictionary<byte, IEnumerable<Point>>> GetAllCellPositionsByStatesAsync() =>
        WithSimulationReadLockAsync(() => _genericCellularAutomation.GetAllCellPositionsByStates());

    protected override void ExecuteCommand(SimulationCommand command)
    {
        
    }
}