using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public interface ISummaryStatsManager<TSummary> : ICollectionManager<SummaryRecord<TSummary>>;