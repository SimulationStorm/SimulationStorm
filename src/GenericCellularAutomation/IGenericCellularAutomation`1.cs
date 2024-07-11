using System;
using System.Collections.Generic;
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
        where TCellState :
            IComparable,
            IComparable<TCellState>,
            IEquatable<TCellState>,
            IBinaryInteger<TCellState>,
            IMinMaxValue<TCellState>
{
    TCellState PossibleCellStateCount { get; set; }
    
    TCellState MaxPossibleCellStateCount { get; }
    
    TCellState DefaultCellState { get; set; }
    
    RuleSetCollection<TCellState> RuleSetCollection { get; set; }

    // Next rule collection index
    int ExecutingRuleCollectionIndex { get; }

    // Next rule index
    int ExecutingRuleIndex { get; }
}