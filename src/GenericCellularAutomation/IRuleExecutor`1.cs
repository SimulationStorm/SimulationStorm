using SimulationStorm.Primitives;

namespace GenericCellularAutomation;

// Todo:
// An idea: Pattern could be renamed to configuration and could be made as Pattern<TCellState>
// Another idea: place patterns in configuration: wire world will include its own config's, GoL its own, etc.

// AdvancementRule
// IAdvancer ? IAdvanceApplier ? IAdvanceExecutor

// + two implementations:
// 1) using a quite slow approach with runtime checks;
// 2) using a runtime code generation
// Or IRuleApplier, IAdvancer
public interface IRuleExecutor<TCellState>
{
    // world - is the previous world
    TCellState CalculateNextCellState(TCellState[,] world, Point cellPosition);
}