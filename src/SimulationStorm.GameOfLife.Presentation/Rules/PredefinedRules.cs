using SimulationStorm.GameOfLife.DataTypes;

namespace SimulationStorm.GameOfLife.Presentation.Rules;

public static class PredefinedRules
{
    public static readonly NamedRule
        // Life family
        GameOfLife = Create(nameof(GameOfLife), "b3/s23"),
        HighLife = Create(nameof(HighLife), "b36/s23"),
        LongLife = Create(nameof(LongLife), "b345/s5"),
        LifeWithoutDeath = Create(nameof(LifeWithoutDeath), "b3/s012345678"),
        // Mazes
        Maze = Create(nameof(Maze), "b3/s12345"),
        Maze2 = Create(nameof(Maze2), "b3/s1234"),
        Maze3 = Create(nameof(Maze3), "b37/s12345"),
        Maze4 = Create(nameof(Maze4), "b37/s1234"),
        // Rugs
        PersianRug = Create(nameof(PersianRug), "b234/s"),
        Rug2 = Create(nameof(Rug2), "b234678/s8"),
        Rug3 = Create(nameof(Rug3), "b2345678/s0238"),
        Rug4 = Create(nameof(Rug4), "b234567/s124567"),
        Rug5 = Create(nameof(Rug5), "b235678/s1234567"),
        // Others
        LiveFreeOrDie = Create(nameof(LiveFreeOrDie), "b2/s0"),
        Seeds = Create(nameof(Seeds), "b2/s"),
        HTrees = Create(nameof(HTrees), "b1/s012345678"),
        Diamoeba = Create(nameof(Diamoeba), "b35678/s5678"),
        DayAndNight = Create(nameof(DayAndNight), "b3678/s34678"),
        Assimilation = Create(nameof(Assimilation), "b345/s4567"),
        Corals = Create(nameof(Corals), "b3/s45678"),
        Coagulation = Create(nameof(Coagulation), "b378/s235678"),
        Majority = Create(nameof(Majority), "b45678/s5678"),
        Annealing = Create(nameof(Annealing), "b378/s235678");

    private static NamedRule Create(string name, string ruleString) => new(name, GameOfLifeRule.FromString(ruleString));
}