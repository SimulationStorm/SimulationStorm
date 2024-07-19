using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Management;

public sealed class GenericCellularAutomationManager : SimulationManagerBase
{
    #region Properties
    public Size WorldSize => _gca.WorldSize;
    
    public CellStateCollection PossibleCellStateCollection { get; set; }
    #endregion

    private readonly IGenericCellularAutomation _gca;
    
    public GenericCellularAutomationManager
    (
        IBenchmarker benchmarker,
        IGenericCellularAutomationFactory gcaFactory
    )
        : base(benchmarker)
    {
        
    }

    public Task<IReadOnlyDictionary<byte, IEnumerable<Point>>> GetAllCellPositionsByStatesAsync() =>
        WithSimulationReadLockAsync(() => _gca.GetAllCellPositionsByStates());

    protected override void ExecuteCommand(SimulationCommand command)
    {
        
    }
}