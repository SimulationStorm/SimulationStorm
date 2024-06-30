using System;

namespace SimulationStorm.Collections.Extensions;

public static class RangeExtensions
{
    public static int Length(this Range range)
    {
        int start = range.Start.IsFromEnd ? ~range.Start.Value : range.Start.Value,
            end = range.End.IsFromEnd ? ~range.End.Value : range.End.Value,
            length = end - start;
        
        return Math.Max(length, 0);
    }

    public static int StartIndex(this Range range) => range.Start.Value;
    
    public static int EndIndex(this Range range) => range.End.Value;
}