using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace GenericCellularAutomation.Presentation.Management.Commands;

public sealed class ChangeCellStateCollectionCommand(CellStateCollection newCellStateCollection)
    : SimulationCommand("ChangeCellStateCollection", true)
{
    public CellStateCollection NewCellStateCollection { get; } = newCellStateCollection;
}