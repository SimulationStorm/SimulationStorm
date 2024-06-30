using System;

namespace SimulationStorm.Utilities.Benchmarking;

public class BenchmarkResult(TimeSpan elapsedTime)
{
    public TimeSpan ElapsedTime { get; } = elapsedTime;
}