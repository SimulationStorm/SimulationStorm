using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace SimulationStorm.Simulation.CellularAutomation.Avalonia.Converters;

public class WorldWrappingToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not WorldWrapping worldWrapping)
            return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);

        return worldWrapping switch
        {
            WorldWrapping.NoWrap => "not wrapped",
            WorldWrapping.Horizontal => "horizontal",
            WorldWrapping.Vertical => "vertical",
            WorldWrapping.Both => "both",
            _ => throw new InvalidEnumArgumentException(nameof(worldWrapping), (int)worldWrapping, typeof(WorldWrapping))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
}