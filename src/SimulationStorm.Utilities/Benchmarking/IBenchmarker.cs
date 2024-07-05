using System;
using System.Threading.Tasks;

namespace SimulationStorm.Utilities.Benchmarking;

public interface IBenchmarker
{
    BenchmarkResult Measure(Action action);

    BenchmarkResult<T> Measure<T>(Func<T> function);
    
    Task<BenchmarkResult> MeasureAsync(Func<Task> asyncAction);
    
    Task<BenchmarkResult<T>> MeasureAsync<T>(Func<Task<T>> asyncFunction);
}