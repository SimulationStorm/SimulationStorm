using System.Collections.Generic;

namespace SimulationStorm.GameOfLife.Presentation.Rules;

public static class PredefinedRuleCategories
{
    public static readonly NamedRuleCategory
        LifeFamily = new(nameof(LifeFamily), new[]
        {
            PredefinedRules.GameOfLife,
            PredefinedRules.HighLife,
            PredefinedRules.LongLife,
            PredefinedRules.LifeWithoutDeath
        }),
        Mazes = new(nameof(Mazes), new[]
        {
            PredefinedRules.Maze,
            PredefinedRules.Maze2,
            PredefinedRules.Maze3,
            PredefinedRules.Maze4
        }),
        Rugs = new(nameof(Rugs), new[]
        {
            PredefinedRules.PersianRug,
            PredefinedRules.Rug2,
            PredefinedRules.Rug3,
            PredefinedRules.Rug4,
            PredefinedRules.Rug5
        }),
        Others = new(nameof(Others), new[]
        {
            PredefinedRules.LiveFreeOrDie,
            PredefinedRules.Seeds,

            PredefinedRules.HTrees,
            PredefinedRules.Diamoeba,
            PredefinedRules.DayAndNight,
            PredefinedRules.Assimilation,
            PredefinedRules.Corals,
            PredefinedRules.Coagulation,
            PredefinedRules.Majority,
            PredefinedRules.Annealing
        });
    
    public static readonly IEnumerable<NamedRuleCategory> All = new[]
    {
        LifeFamily,
        Mazes,
        Rugs,
        Others
    };
}