using System.Collections.Generic;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Neighborhood;
using GenericCellularAutomation.Presentation.Patterns;
using GenericCellularAutomation.Presentation.Rules.Descriptors;
using GenericCellularAutomation.Rules;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.Configurations;

public static class GameOfLifeConfigurationBuilder
{
    private static readonly CellStateDescriptor DeadCell = new CellStateDescriptorBuilder()
        .HasCellState(1)
        .HasName("Dead")
        .HasColor(KnownColors.Black)
        .Build();
    
    private static readonly CellStateDescriptor AliveCell = new CellStateDescriptorBuilder()
        .HasCellState(1)
        .HasName("Alive")
        .HasColor(KnownColors.White)
        .Build();
    
    private static readonly IReadOnlyDictionary<string, CellStateDescriptor> CellStateByNames =
        new Dictionary<string, CellStateDescriptor>
    {
        [" "] = DeadCell,
        ["+"] = AliveCell
    };
    
    public static Configuration Build() => new ConfigurationBuilder()
        .HasName("GameOfLife")
        .HasCellStateCollection(
            new CellStateCollectionDescriptorBuilder()
                .HasCellStates(DeadCell, AliveCell)
                .HasDefaultCellState(DeadCell)
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
                                .HasTargetCellState(DeadCell)
                                .HasNewCellState(AliveCell)
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
                                .HasTargetCellState(DeadCell)
                                .HasNewCellState(AliveCell)
                                .HasApplicationProbability(1)
                                .HasNeighborCellState(AliveCell)
                                .HasCellNeighborhood(
                                    PredefinedNeighborhoodTemplates.Moore
                                        .BuildNeighborhood(radius: 1))
                                .HasNeighborCellCount(3)
                                .Build(),
                            new RuleDescriptorBuilder()
                                .HasName("SurvivalRule")
                                .HasType(RuleType.Totalistic)
                                .HasTargetCellState(AliveCell)
                                .HasNewCellState(DeadCell)
                                .HasApplicationProbability(1)
                                .HasCellNeighborhood(
                                    PredefinedNeighborhoodTemplates.Moore
                                        .BuildNeighborhood(radius: 1))
                                .HasNeighborCellState(DeadCell)
                                .HasNeighborCellCount(0, 1, 2, 3, 4, 7, 8) // excluding 5 and 6
                                .Build())
                        .Build())
                .Build())
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
                        .HasSchemeCellStateNames(CellStateByNames)
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
                        .HasSchemeCellStateNames(CellStateByNames)
                        .Build())
                .Build())
        .Build();
}