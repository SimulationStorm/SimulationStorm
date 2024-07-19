using System.Collections.Generic;
using GenericCellularAutomation.Rules;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.History;
using SimulationStorm.Simulation.Resetting;
using SimulationStorm.Simulation.Running;
using SimulationStorm.Simulation.Statistics;

namespace GenericCellularAutomation;

public interface IGenericCellularAutomation :
    IAdvanceableSimulation,
    IResettableSimulation,
    ISummarizableSimulation<GenericCellularAutomationSummary>,
    ISaveableSimulation<GenericCellularAutomationSave>,
    IBoundedCellularAutomation<byte>
{
    CellStateCollection PossibleCellStateCollection { get; set; }
    
    RuleSetCollection RuleSetCollection { get; set; }

    // Todo: index incrementing and resetting
    int NextExecutingRuleSetIndex { get; }

    IReadOnlyDictionary<byte, IEnumerable<Point>> GetAllCellPositionsByStates();
}