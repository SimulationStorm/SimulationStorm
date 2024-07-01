using System;

namespace SimulationStorm.Utilities.Benchmarking;

public readonly struct BenchmarkResult(TimeSpan elapsedTime)
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
}