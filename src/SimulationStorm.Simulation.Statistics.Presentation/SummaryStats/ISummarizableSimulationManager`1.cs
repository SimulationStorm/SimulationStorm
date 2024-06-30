using System.Threading.Tasks;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public interface ISummarizableSimulationManager<TSummary> : ISimulationManager
{
    Task<BenchmarkResult<TSummary>> SummarizeAndMeasureAsync();
}