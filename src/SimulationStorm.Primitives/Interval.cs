using System;
using System.Text.Json.Serialization;

namespace SimulationStorm.Primitives;

public readonly struct Interval<T> where T : struct, IComparable<T>
{
    public T Minimum { get; }
    
    public T Maximum { get; }

    [JsonConstructor]
    public Interval(T minimum, T maximum)
    {
        if (minimum.CompareTo(maximum) > 0)
            throw new ArgumentException("Minimum value cannot be greater than maximum value.", nameof(minimum));

        Minimum = minimum;
        Maximum = maximum;
    }
}