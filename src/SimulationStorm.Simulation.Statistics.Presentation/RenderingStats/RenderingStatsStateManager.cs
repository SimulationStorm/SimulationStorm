using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;

public class RenderingStatsStateManager(IRenderingStatsManager renderingStatsManager)
    : CollectionStateManagerBase<RenderingResultRecord, RenderingStatsState>(renderingStatsManager);