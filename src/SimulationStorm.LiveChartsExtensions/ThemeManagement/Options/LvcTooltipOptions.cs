using LiveChartsCore.Drawing;
using SimulationStorm.LiveChartsExtensions.FontManagement;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

public class LvcTooltipOptions
{
    public LvcFont Font { get; init; }

    public int FontSize { get; init; }

    public LvcColor ForegroundColor { get; init; }

    public LvcColor BackgroundColor { get; init; }
}