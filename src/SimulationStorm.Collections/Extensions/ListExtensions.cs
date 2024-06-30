using System.Collections.Generic;
using DynamicData;

namespace SimulationStorm.Collections.Extensions;

public static class ListExtensions
{
    public static void RemoveRange<T>(this IList<T> list, int index, int count)
    {
        switch (list)
        {
            case List<T> listImpl:
            {
                listImpl.RemoveRange(index, count);
                break;
            }
            case IExtendedList<T> extendedList:
            {
                extendedList.RemoveRange(index, count);
                break;
            }
            default:
            {
                for (var i = 0; i < count; i++)
                    list.RemoveAt(index);
                break;
            }
        }
    }
}