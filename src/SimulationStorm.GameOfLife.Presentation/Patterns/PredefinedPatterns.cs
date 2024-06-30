using SimulationStorm.GameOfLife.DataTypes;

namespace SimulationStorm.GameOfLife.Presentation.Patterns;

public static class PredefinedPatterns
{
    public static readonly NamedPattern
        // Still life
        Block = Create(nameof(Block),
            """
            XX
            XX
            """
        ),
        Beehive = Create(nameof(Beehive),
            """
            -XX-
            X--X
            -XX-
            """
        ),
        Loaf = Create(nameof(Loaf),
            """
            -XX-
            X--X
            -X-X
            --X-
            """
        ),
        Tub = Create(nameof(Tub),
            """
            -X-
            X-X
            -X-
            """
        ),
        Boat = Create(nameof(Boat),
            """
            XX-
            X-X
            -X-
            """
        ),
        Carrier = Create(nameof(Carrier),
            """
            XX--
            X--X
            --XX
            """
        ),
        // Oscillators
        Blinker = Create(nameof(Blinker),
            """
            XXX
            """
        ),
        FigureEight = Create(nameof(FigureEight),
            """
            XX----
            XX-X--
            ----X-
            -X----
            --X-XX
            ----XX
            """
        ),
        Octagon = Create(nameof(Octagon),
            """
            ---XX---
            --X--X--
            -X----X-
            X------X
            X------X
            -X----X-
            --X--X--
            ---XX---
            """
        ),
        Pentadecathlon = Create(nameof(Pentadecathlon),
            """
            --X----X--
            XX-XXXX-XX
            --X----X--
            """
        ),
        SparkCoil = Create(nameof(SparkCoil),
            """
            XX----XX
            X-X--X-X
            --X--X--
            X-X--X-X
            XX----XX
            """
        ),
        Wheel = Create(nameof(Wheel),
            """
            ------XX----
            ------XX----
            ------------
            ----XXXX----
            XX-X----X---
            XX-X--X-X---
            ---X---XX-XX
            ---X-X--X-XX
            ----XXXX----
            ------------
            ----XX------
            ----XX------
            """
        ),
        // Spaceships
        LightweightSpaceship = Create(nameof(LightweightSpaceship),
            """
            -X--X
            X----
            X---X
            XXXX-
            """
        ),
        MediumweightSpaceship = Create(nameof(MediumweightSpaceship),
            """
            ---X--
            -X---X
            X-----
            X----X
            XXXXX-
            """
        ),
        HeavyweightSpaceship = Create(nameof(HeavyweightSpaceship),
            """
            ---XX--
            -X----X
            X------
            X-----X
            XXXXXX-
            """
        ),
        Glider = Create(nameof(Glider),
            """
            -X-
            --X
            XXX
            """
        ),
        Loafer = Create(nameof(Loafer),
            """
            -XX--X-XX
            X--X--XX-
            -X-X-----
            --X------
            --------X
            ------XXX
            -----X---
            ------X--
            -------XX
            """
        ),
        Moon = Create(nameof(Moon),
            """
            -X
            X-
            X-
            -X
            """
        ),
        // Guns
        GliderGun = Create(nameof(GliderGun),
            """
            ------------------------X-----------
            ----------------------X-X-----------
            ------------XX------XX------------XX
            -----------X---X----XX------------XX
            ----------X-----X---XX--------------
            XX--------X---X-XX----X-X-----------
            XX--------X-----X-------X-----------
            -----------X---X--------------------
            ------------XX----------------------
            """
        ),
        // Methuselahs
        Acorn = Create(nameof(Acorn),
            """
            -X-----
            ---X---
            XX--XXX
            """
        ),
        BHeptomino = Create(nameof(BHeptomino),
            """
            X-XX
            XXX-
            -X--
            """
        ),
        Diehard = Create(nameof(Diehard),
            """
            ------X-
            XX------
            -X---XXX
            """
        ),
        GliderByTheDozen = Create(nameof(GliderByTheDozen),
            """
            XX--X
            X---X
            X--XX
            """
        ),
        PHeptomino = Create(nameof(PHeptomino),
            """
            XXX
            X-X
            X-X
            """
        ),
        Thunderbird = Create(nameof(Thunderbird),
            """
            XXX
            ---
            -X-
            -X-
            -X-
            """
        ),
        // Wicks
        Ants = Create(nameof(Ants),
            """
            XX---XX---XX---XX---XX---XX---XX---XX---XX--
            --XX---XX---XX---XX---XX---XX---XX---XX---XX
            --XX---XX---XX---XX---XX---XX---XX---XX---XX
            XX---XX---XX---XX---XX---XX---XX---XX---XX--
            """
        ),
        BlinkerFuse = Create(nameof(BlinkerFuse),
            """
            XX--X-XX-----------------
            XXXXX-X-X----------------
            --------X-XXX-XXX-XXX-XXX
            XXXXX-X-X----------------
            XX--X-XX-----------------
            """
        ),
        LightWave = Create(nameof(LightWave),
            """
            XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            -XX--XX--XX--XX--XX--XX--XX--XX-
            """
        );

    private static NamedPattern Create(string name, string patternString) =>
        new(name, GameOfLifePattern.FromString(patternString, '-', 'X'));
}