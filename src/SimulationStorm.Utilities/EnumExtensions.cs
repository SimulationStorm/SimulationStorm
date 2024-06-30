using System;

namespace SimulationStorm.Utilities;

public static class EnumExtensions
{
    public static T ToggleFlags<T>(this T @enum, T flags) where T : Enum =>
        @enum.HasFlag(flags) ? @enum.WithoutFlags(flags) : @enum.WithFlags(flags);
    
    public static T WithFlags<T>(this T @enum, T flags) where T : Enum
    {
        T result;
        
        // 'long' can hold all possible values, except those which 'ulong' can hold.
        if (Enum.GetUnderlyingType(typeof(T)) == typeof(ulong))
        {
            var ulongValue = Convert.ToUInt64(@enum);
            ulongValue |= Convert.ToUInt64(flags);
            result = (T)Enum.ToObject(typeof(T), ulongValue);
        }
        else
        {
            var longValue = Convert.ToInt64(@enum);
            longValue |= Convert.ToInt64(flags);
            result = (T)Enum.ToObject(typeof(T), longValue);
        }

        return result;
    }
    
    public static T WithoutFlags<T>(this T @enum, T flags) where T : Enum
    {
        T result;
        
        // 'long' can hold all possible values, except those which 'ulong' can hold.
        if (Enum.GetUnderlyingType(typeof(T)) == typeof(ulong))
        {
            var ulongValue = Convert.ToUInt64(@enum);
            ulongValue &= ~Convert.ToUInt64(flags);
            result = (T)Enum.ToObject(typeof(T), ulongValue);
        }
        else
        {
            var longValue = Convert.ToInt64(@enum);
            longValue &= ~Convert.ToInt64(flags);
            result = (T)Enum.ToObject(typeof(T), longValue);
        }

        return result;
    }
}