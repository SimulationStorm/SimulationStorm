using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace SimulationStorm.Primitives.Avalonia.Converters;

public class SizeToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
    {
        Size size => $"{size.Width} x {size.Height}",
        SizeF size => $"{size.Width} x {size.Height}",
        _ => new BindingNotification(new InvalidCastException(), BindingErrorType.Error)
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
}