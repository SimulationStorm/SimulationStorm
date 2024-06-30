using System.Collections.Generic;
using System.Linq;

namespace SimulationStorm.Collections.Extensions;

public static class EnumerableExtensions
{
    public static T SingleAs<T>(this IEnumerable<object> enumerable) => (T)enumerable.Single();
}