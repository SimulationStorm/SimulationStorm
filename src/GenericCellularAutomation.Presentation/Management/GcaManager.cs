using System.Collections.Generic;
using System.Threading.Tasks;
using GenericCellularAutomation.Presentation.Management.Commands;
using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Management;

public sealed class GcaManager : SimulationManagerBase
{
    #region Properties
    public Size WorldSize => _simulation.WorldSize;
    
    public CellStateCollection CellStateCollection { get; private set; } = null!;
    
    public Range<byte> CellStateRange { get; }
    
    public RuleSetCollection RuleSetCollection { get; private set; } = null!;
    #endregion

    private readonly IGenericCellularAutomation _simulation;
    
    public GcaManager
    (
        IBenchmarker benchmarker,
        IRuleExecutorFactory ruleExecutorFactory,
        IGcaFactory gcaFactory,
        GcaOptions options
    )
        : base(benchmarker)
    {
        _simulation = gcaFactory.CreateGenericCellularAutomation
        (
            ruleExecutorFactory,
            options.WorldSize,
            options.MaxCellNeighborhoodRadius,
            options.MaxCellState,
            options.WorldWrapping,
            options.Configuration.CellStateCollection.ToCellStateCollection(),
            options.Configuration.RuleSetCollection.ToRuleSetCollection()
        );
        
        RememberSimulationPropertyValues();
        ResetSimulationInstance(_simulation);
    }

    #region Reading methods
    public Task<IReadOnlyDictionary<byte, IEnumerable<Point>>> GetAllCellPositionsByStatesAsync() =>
        WithSimulationReadLockAsync(() => _simulation.GetAllCellPositionsByStates());
    #endregion

    #region Writing methods
    public Task ChangeCellStateCollectionAsync(CellStateCollection newCellStateCollection) =>
        ScheduleCommandAsync(new ChangeCellStateCollectionCommand(newCellStateCollection));

    public Task ChangeRuleSetCollectionAsync(RuleSetCollection newRuleSetCollection) =>
        ScheduleCommandAsync(new ChangeRuleSetCollectionCommand(newRuleSetCollection));
    #endregion

    #region Commands execution
    protected override void ExecuteCommand(SimulationCommand command)
    {
        switch (command)
        {
            case ChangeCellStateCollectionCommand changeCellStateCollectionCommand:
            {
                ExecuteChangeCellStateCollection(changeCellStateCollectionCommand);
                break;
            }
            case ChangeRuleSetCollectionCommand changeRuleSetCollectionCommand:
            {
                ExecuteChangeRuleSetCollection(changeRuleSetCollectionCommand);
                break;
            }
        }
    }

    private void ExecuteChangeCellStateCollection(ChangeCellStateCollectionCommand command)
    {
        _simulation.CellStateCollection = command.NewCellStateCollection;
        CellStateCollection = _simulation.CellStateCollection;
    }
    
    private void ExecuteChangeRuleSetCollection(ChangeRuleSetCollectionCommand command)
    {
        _simulation.RuleSetCollection = command.NewRuleSetCollection;
        RuleSetCollection = _simulation.RuleSetCollection;
    }
    #endregion

    private void RememberSimulationPropertyValues()
    {
        CellStateCollection = _simulation.CellStateCollection;
        RuleSetCollection = _simulation.RuleSetCollection;
    }
}