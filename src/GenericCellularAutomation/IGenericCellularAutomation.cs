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
    ISummarizableSimulation<GcaSummary>,
    ISaveableSimulation<GcaSave>,
    IBoundedCellularAutomation<byte>
{
    #region Properties
    /// <summary>
    /// Gets the minimum and the maximum possible cell state.
    /// </summary>
    Range<byte> CellStateRange { get; }
    
    /// <summary>
    /// Gets or sets the possible cell state collection.
    /// </summary>
    CellStateCollection CellStateCollection { get; set; }

    /// <summary>
    /// Gets or sets the rule set collection.
    /// </summary>
    RuleSetCollection RuleSetCollection { get; set; }

    /// <summary>
    /// Gets the index of the rule set that will be executed next
    /// </summary>
    int NextExecutingRuleSetIndex { get; }
    #endregion

    IReadOnlyDictionary<byte, IEnumerable<Point>> GetAllCellPositionsByStates();
}