using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SimulationStorm.Avalonia.Converters;

public class TypeConverter<TFrom, TTo>(Func<TFrom, TTo> fromToConverter, Func<TTo, TFrom> toFromConverter) : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is not TFrom fromValue ? default : fromToConverter(fromValue);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is not TTo toValue ? default : toFromConverter(toValue);
}