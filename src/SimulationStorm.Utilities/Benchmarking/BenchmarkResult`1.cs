using System;

namespace SimulationStorm.Utilities.Benchmarking;

public readonly struct BenchmarkResult<T>(TimeSpan elapsedTime, T functionResult)
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
    
    public T FunctionResult { get; } = functionResult;
}