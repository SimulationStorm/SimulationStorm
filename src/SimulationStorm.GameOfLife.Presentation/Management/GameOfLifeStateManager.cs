using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.Simulation.History.Presentation.Services;

namespace SimulationStorm.GameOfLife.Presentation.Management;

public class GameOfLifeSaveManager(GameOfLifeManager gameOfLifeManager) :
    SimulationSaveManagerBase<GameOfLifeSave>(gameOfLifeManager);