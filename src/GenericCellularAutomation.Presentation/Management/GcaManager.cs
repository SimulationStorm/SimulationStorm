using System.Collections.Generic;
using System.Threading.Tasks;
using GenericCellularAutomation.Presentation.Management.Commands;
using GenericCellularAutomation.RuleExecution;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Commands;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Commands;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Resetting.Presentation.Commands;
using SimulationStorm.Simulation.Resetting.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.Commands;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Management;

public sealed class GcaManager : SimulationManagerBase,
    ISaveableSimulationManager<GcaSave>,
    ISummarizableSimulationManager<GcaSummary>,
    IAdvanceableSimulationManager,
    IBoundedSimulationManager,
    IResettableSimulationManager,
    IBoundedCellularAutomationManager<GcaCellState>
{
    #region Properties
    public Size WorldSize { get; private set; }
    
    public WorldWrapping WorldWrapping { get; private set; }

    public CellStateCollection CellStateCollection { get; private set; } = null!;

    public Range<byte> CellStateRange => _simulation.CellStateRange;
    
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
    public Task<GcaSave> SaveAsync() =>
        WithSimulationReadLockAsync(_simulation.Save);
    
    public Task<BenchmarkResult<GcaSave>> SaveAndMeasureAsync() =>
        MeasureWithSimulationReadLockAsync(_simulation.Save);

    public Task<GcaSummary> SummarizeAsync() =>
        WithSimulationReadLockAsync(_simulation.Summarize);
    
    public Task<BenchmarkResult<GcaSummary>> SummarizeAndMeasureAsync() =>
        MeasureWithSimulationReadLockAsync(_simulation.Summarize);

    public Task<IReadOnlyDictionary<byte, IEnumerable<Point>>> GetAllCellPositionsByStatesAsync() =>
        WithSimulationReadLockAsync(() => _simulation.GetAllCellPositionsByStates());
    #endregion

    #region Writing methods
    public Task ChangeWorldSizeAsync(Size newSize) =>
        ScheduleCommandAsync(new ChangeWorldSizeCommand(newSize));
    
    public Task ChangeWorldWrappingAsync(WorldWrapping newWrapping) =>
        ScheduleCommandAsync(new ChangeWorldWrappingCommand(newWrapping));

    public Task RestoreSaveAsync(GcaSave save, bool isRestoringFromAppSave = false) =>
        ScheduleCommandAsync(new RestoreSaveCommand(save, isRestoringFromAppSave));
    
    public Task AdvanceAsync() =>
        ScheduleCommandAsync(new AdvanceCommand());

    public Task ResetAsync() =>
        ScheduleCommandAsync(new ResetCommand());
    
    public Task ChangeCellStateCollectionAsync(CellStateCollection newCellStateCollection) =>
        ScheduleCommandAsync(new ChangeCellStateCollectionCommand(newCellStateCollection));

    public Task ChangeRuleSetCollectionAsync(RuleSetCollection newRuleSetCollection) =>
        ScheduleCommandAsync(new ChangeRuleSetCollectionCommand(newRuleSetCollection));

    public Task ChangeCellStatesAsync(IEnumerable<Point> cells, GcaCellState newState) =>
        ScheduleCommandAsync(new DrawCommand<GcaCellState>(cells, newState));
    #endregion

    #region Commands execution
    protected override void ExecuteCommand(SimulationCommand command)
    {
        switch (command)
        {
            case ChangeWorldSizeCommand changeWorldSizeCommand:
            {
                ExecuteChangeWorldSize(changeWorldSizeCommand);
                break;
            }
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

    private void ExecuteChangeWorldSize(ChangeWorldSizeCommand command)
    {
        _simulation.ChangeWorldSize(command.NewSize);
        WorldSize = _simulation.WorldSize;
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
        WorldSize = _simulation.WorldSize;
        CellStateCollection = _simulation.CellStateCollection;
        RuleSetCollection = _simulation.RuleSetCollection;
    }
}