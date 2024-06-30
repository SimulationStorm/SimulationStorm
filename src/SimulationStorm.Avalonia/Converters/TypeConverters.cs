using System;
using Avalonia.Data.Converters;

namespace SimulationStorm.Avalonia.Converters;

public static class TypeConverters
{
    public static readonly IValueConverter IntToDoubleConverter = new TypeConverter<int, double>
        (intNumber => intNumber, doubleNumber => (int)Math.Round(doubleNumber));
    
    public static readonly IValueConverter IntToDecimalConverter = new TypeConverter<int, decimal>
        (intNumber => intNumber, decimalNumber => (int)Math.Round(decimalNumber));
    
    public static readonly IValueConverter FloatToDoubleConverter = new TypeConverter<float, double>
        (floatNumber => floatNumber, doubleNumber => (float)doubleNumber);
    
    public static readonly IValueConverter FloatToDecimalConverter = new TypeConverter<float, decimal>
        (floatNumber => (decimal)floatNumber, decimalNumber => (float)decimalNumber);
}