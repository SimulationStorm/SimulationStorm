using Avalonia.Data.Converters;

namespace SimulationStorm.Graphics.Avalonia.Converters;

public static class GraphicsConverters
{
    public static readonly IValueConverter ColorToAvaloniaConverter = new ColorToAvaloniaConverter();
}