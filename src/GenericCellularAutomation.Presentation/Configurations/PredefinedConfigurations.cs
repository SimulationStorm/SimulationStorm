using System.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Neighborhood;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules.Descriptors;
using GenericCellularAutomation.Rules;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.Configurations;

public static class PredefinedConfigurations
{
    public static readonly Configuration
        GameOfLife = BuildGameOfLife();

    private static Configuration BuildGameOfLife()
    {
        var deadCell = new CellStateDescriptorBuilder()
            .HasCellState(1)
            .HasName("Dead")
            .HasColor(KnownColors.Black)
            .Build();
        
        var aliveCell = new CellStateDescriptorBuilder()
            .HasCellState(2)
            .HasName("Alive")
            .HasColor(KnownColors.White)
            .Build();

        var cellStatesByName = new Dictionary<string, CellStateDescriptor>
        {
            [" "] = deadCell,
            ["+"] = aliveCell
        };

        return new ConfigurationBuilder()
            .HasName("GameOfLife")
            .HasPossibleCellStateCollection(
                new CellStateCollectionDescriptorBuilder()
                    .HasCellStates(deadCell, aliveCell)
                    .HasDefaultCellState(deadCell)
                    .Build())
            .HasRuleSetCollection(
                new RuleSetCollectionDescriptorBuilder()
                    .HasRepetitionCount(1)
                    .HasRuleSets(
                        new RuleSetDescriptorBuilder()
                            .HasRepetitionCount(1)
                            .HasName("HelperRuleSet")
                            .HasRules(
                                new RuleDescriptorBuilder()
                                    .HasName("PopulateRandomly")
                                    .HasType(RuleType.Unconditional)
                                    .HasTargetCellState(deadCell)
                                    .HasNewCellState(aliveCell)
                                    .HasApplicationProbability(0.5)
                                    .Build())
                            .Build(),
                        new RuleSetDescriptorBuilder()
                            .HasName("ActualRules")
                            .HasRepetitionCount(1)
                            .HasRules(
                                new RuleDescriptorBuilder()
                                    .HasName("BornRule")
                                    .HasType(RuleType.Totalistic)
                                    .HasTargetCellState(deadCell)
                                    .HasNewCellState(aliveCell)
                                    .HasApplicationProbability(1)
                                    .HasNeighborCellState(aliveCell)
                                    .HasCellNeighborhood(
                                        PredefinedNeighborhoodTemplates.Moore
                                            .BuildNeighborhood(radius: 1))
                                    .HasNeighborCellCount(3)
                                    .Build(),
                                new RuleDescriptorBuilder()
                                    .HasName("SurvivalRule")
                                    .HasType(RuleType.Totalistic)
                                    .HasTargetCellState(aliveCell)
                                    .HasNewCellState(deadCell)
                                    .HasApplicationProbability(1)
                                    .HasCellNeighborhood(
                                        PredefinedNeighborhoodTemplates.Moore
                                            .BuildNeighborhood(radius: 1))
                                    .HasNeighborCellState(deadCell)
                                    .HasNeighborCellCount(0, 1, 2, 3, 4, 7, 8) // excluding 5 and 6
                                    .Build())
                            .Build())
                    .Build()
                )
            .HasPatternCategories(
                new PatternDescriptorCategoryBuilder()
                    .HasName("StaticPatterns")
                    .HasPatterns(
                        new PatternDescriptorBuilder()
                            .HasName("Block")
                            .HasScheme("""
                                       ++
                                       ++
                                       """)
                            .HasSchemeCellStateNames(cellStatesByName)
                            .Build())
                    .Build(),
                new PatternDescriptorCategoryBuilder()
                    .HasName("Oscillators")
                    .HasPatterns(
                        new PatternDescriptorBuilder()
                            .HasName("Test")
                            .HasScheme("""
                                        |+| 
                                       +| |+
                                       +| |+
                                       """)
                            .HasSchemeCellStateNames(cellStatesByName)
                            .Build())
                    .Build()
                )
            .Build();
    }
}