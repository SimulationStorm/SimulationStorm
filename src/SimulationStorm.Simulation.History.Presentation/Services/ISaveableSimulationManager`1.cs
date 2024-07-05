using System.Threading.Tasks;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public interface ISaveableSimulationManager<TSave> : ISimulationManager
{
    Task<TSave> SaveAsync();

    Task<BenchmarkResult<TSave>> SaveAndMeasureAsync();
    
    Task RestoreSaveAsync(TSave save, bool isRestoringFromAppSave = false);
}