using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.History.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Management;

public class GameOfLifeStateManager(GameOfLifeManager gameOfLifeManager) :
    SimulationStateManagerBase<GameOfLifeSave>(gameOfLifeManager);