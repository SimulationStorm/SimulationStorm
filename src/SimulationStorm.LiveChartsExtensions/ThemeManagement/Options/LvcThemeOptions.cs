using System;
using System.Collections.Generic;
using LiveChartsCore.Drawing;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

public class LvcThemeOptions
{
    public TimeSpan AnimationsSpeed { get; init; }

    public Func<float, float> EasingFunction { get; init; } = null!;
    
    public IReadOnlyCollection<LvcColor> ColorPalette { get; init; } = null!;

    public LvcLegendOptions LegendOptions { get; init; } = null!;
    
    public LvcTooltipOptions TooltipOptions { get; init; } = null!;

    public LvcAxisOptions AxisOptions { get; init; } = null!;

    public LvcLineSeriesOptions LineSeriesOptions { get; init; } = null!;

    public LvcBarSeriesOptions BarSeriesOptions { get; init; } = null!;
}