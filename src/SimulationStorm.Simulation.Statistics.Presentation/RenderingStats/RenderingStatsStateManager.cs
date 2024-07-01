using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;

public class RenderingStatsSaveManager(IRenderingStatsManager renderingStatsManager)
    : CollectionSaveManagerBase<RenderingResultRecord, RenderingStatsSave>(renderingStatsManager);