using System.Numerics;
using GenericCellularAutomation.Rules;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.History;
using SimulationStorm.Simulation.Resetting;
using SimulationStorm.Simulation.Running;
using SimulationStorm.Simulation.Statistics;

namespace GenericCellularAutomation;

public interface IGenericCellularAutomation<TCellState> :
    IAdvanceableSimulation,
    IResettableSimulation,
    ISummarizableSimulation<GenericCellularAutomationSummary<TCellState>>,
    ISaveableSimulation<GenericCellularAutomationSave<TCellState>>,
    IBoundedCellularAutomation<TCellState>
    where TCellState : IBinaryInteger<TCellState>
{
    CellStateCollection<TCellState> PossibleCellStateCollection { get; set; }
    
    RuleSetCollection<TCellState> RuleSetCollection { get; set; }

    // Todo: index incrementing and resetting
    int NextExecutingRuleSetIndex { get; }
}