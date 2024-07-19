using System.Collections.Generic;
using System.Threading.Tasks;
using GenericCellularAutomation.Presentation.Management.Commands;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Management;

public sealed class GenericCellularAutomationManager : SimulationManagerBase
{
    #region Properties
    public Size WorldSize => _gca.WorldSize;
    
    public CellStateCollection CellStateCollection { get; }
    
    public RuleSetCollection RuleSetCollection { get; }
    #endregion

    private readonly IGenericCellularAutomation _gca;
    
    public GenericCellularAutomationManager
    (
        IBenchmarker benchmarker,
        IGenericCellularAutomationFactory gcaFactory
    )
        : base(benchmarker)
    {
        _gca = gcaFactory.CreateGenericCellularAutomation();
    }

    #region Reading methods
    public Task<IReadOnlyDictionary<byte, IEnumerable<Point>>> GetAllCellPositionsByStatesAsync() =>
        WithSimulationReadLockAsync(() => _gca.GetAllCellPositionsByStates());
    #endregion

    #region Writing methods
    public Task ChangeCellStateCollectionAsync(CellStateCollection newCellStateCollection) =>
        ScheduleCommandAsync(new ChangeCellStateCollectionCommand(newCellStateCollection));

    public Task ChangeRuleSetCollectionAsync(RuleSetCollection newRuleSetCollection) =>
        ScheduleCommandAsync(new ChangeRuleSetCollectionCommand(newRuleSetCollection));
    #endregion
    
    protected override void ExecuteCommand(SimulationCommand command)
    {
        
    }
}