using System;

namespace SimulationStorm.Utilities.Benchmarking;

public class BenchmarkResult<T>(TimeSpan elapsedTime, T functionResult) : BenchmarkResult(elapsedTime)
{
    public T FunctionResult { get; } = functionResult;
}