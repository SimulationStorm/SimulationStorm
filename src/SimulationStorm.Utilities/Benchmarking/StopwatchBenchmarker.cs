using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimulationStorm.Utilities.Benchmarking;

public class StopwatchBenchmarker : IBenchmarker
{
    private readonly Stopwatch _stopwatch = new();
    
    public BenchmarkResult Measure(Action action)
    {
        _stopwatch.Restart();
        action();
        _stopwatch.Stop();
        return new BenchmarkResult(_stopwatch.Elapsed);
    }

    public BenchmarkResult<T> Measure<T>(Func<T> function)
    {
        _stopwatch.Restart();
        var functionResult = function();
        _stopwatch.Stop();
        return new BenchmarkResult<T>(_stopwatch.Elapsed, functionResult);
    }

    public async Task<BenchmarkResult> MeasureAsync(Func<Task> asyncAction)
    {
        _stopwatch.Restart();
        
        await asyncAction()
            .ConfigureAwait(false);
        
        _stopwatch.Stop();
        
        return new BenchmarkResult(_stopwatch.Elapsed);
    }

    public async Task<BenchmarkResult<T>> MeasureAsync<T>(Func<Task<T>> asyncFunction)
    {
        _stopwatch.Restart();

        var taskResult = await asyncFunction()
            .ConfigureAwait(false);
        
        _stopwatch.Stop();
        
        return new BenchmarkResult<T>(_stopwatch.Elapsed, taskResult);
    }
}