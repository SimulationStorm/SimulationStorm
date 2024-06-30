using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using SimulationStorm.Graphics.Avalonia.Extensions;
using AvaloniaColor = Avalonia.Media.Color;

namespace SimulationStorm.Graphics.Avalonia.Converters;

public class ColorToAvaloniaConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(value, targetType);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(value, targetType);

    private static object Convert(object? value, Type targetType) => value switch
    {
        Color color when targetType == typeof(Color) => color,
        Color color when targetType == typeof(AvaloniaColor) => color.ToAvalonia(),
        AvaloniaColor avaloniaColor when targetType == typeof(AvaloniaColor) => avaloniaColor,
        AvaloniaColor avaloniaColor when targetType == typeof(Color) => avaloniaColor.ToColor(),
        _ => new BindingNotification(new InvalidCastException(), BindingErrorType.Error)
    };
}