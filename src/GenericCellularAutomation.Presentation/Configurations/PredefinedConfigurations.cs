using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Neighborhood;
using GenericCellularAutomation.Presentation.Rules.Descriptors;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.Configurations;

public static class PredefinedConfigurations
{
    public static readonly Configuration
        GameOfLife = BuildGameOfLife();

    private static Configuration BuildGameOfLife()
    {
        var deadCell = new CellStateDescriptorBuilder()
            .HasName("Dead")
            .HasColor(KnownColors.Black)
            .Build();
        
        var aliveCell = new CellStateDescriptorBuilder()
            .HasName("Alive")
            .HasColor(KnownColors.White)
            .Build();

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
                                    .HasTargetCellState(deadCell)
                                    .HasNewCellState(aliveCell)
                                    .HasNeighborCellState(aliveCell)
                                    .HasCellNeighborhood(
                                        PredefinedNeighborhoodTemplates.Moore
                                            .BuildNeighborhood(radius: 1))
                                    .HasNeighborCellCount(3)
                                    .Build(),
                                new RuleDescriptorBuilder()
                                    .HasName("SurvivalRule")
                                    .HasTargetCellState(aliveCell)
                                    .HasNewCellState(deadCell)
                                    .HasCellNeighborhood(
                                        PredefinedNeighborhoodTemplates.Moore
                                            .BuildNeighborhood(radius: 1))
                                    .HasNeighborCellState(deadCell)
                                    .HasNeighborCellCount(0, 1, 2, 3, 4, 7, 8) // excluding 5 and 6
                                    .Build())
                            .Build())
                    .Build()
                )
            .HasPatternCategories()
            .Build();
    }
}