using System.Collections.Generic;

namespace SimulationStorm.GameOfLife.Presentation.Patterns;

public static class PredefinedPatternCategories
{
    private static readonly NamedPatternCategory
        StillLifes = new(nameof(StillLifes), new[]
        {
            PredefinedPatterns.Block,
            PredefinedPatterns.Beehive,
            PredefinedPatterns.Loaf,
            PredefinedPatterns.Tub,
            PredefinedPatterns.Boat,
            PredefinedPatterns.Carrier
        }),
        Oscillators = new(nameof(Oscillators), new[]
        {
            PredefinedPatterns.Blinker,
            PredefinedPatterns.FigureEight,
            PredefinedPatterns.Octagon,
            PredefinedPatterns.Pentadecathlon,
            PredefinedPatterns.SparkCoil,
            PredefinedPatterns.Wheel
        }),
        Spaceships = new(nameof(Spaceships), new[]
        {
            PredefinedPatterns.LightweightSpaceship,
            PredefinedPatterns.MediumweightSpaceship,
            PredefinedPatterns.HeavyweightSpaceship,
            PredefinedPatterns.Glider,
            PredefinedPatterns.Loafer,
            PredefinedPatterns.Moon
        }),
        Guns = new(nameof(Guns), new[]
        {
            PredefinedPatterns.GliderGun
        }),
        Methuselahs = new(nameof(Methuselahs), new[]
        {
            PredefinedPatterns.Acorn,
            PredefinedPatterns.BHeptomino,
            PredefinedPatterns.PHeptomino,
            PredefinedPatterns.Diehard,
            PredefinedPatterns.GliderByTheDozen,
            PredefinedPatterns.Thunderbird
        }),
        Wicks = new(nameof(Wicks), new[]
        {
            PredefinedPatterns.Ants,
            PredefinedPatterns.BlinkerFuse,
            PredefinedPatterns.LightWave
        });

    public static readonly IEnumerable<NamedPatternCategory> All = new[]
    {
        StillLifes,
        Oscillators,
        Spaceships,
        Guns,
        Methuselahs,
        Wicks
    };
}