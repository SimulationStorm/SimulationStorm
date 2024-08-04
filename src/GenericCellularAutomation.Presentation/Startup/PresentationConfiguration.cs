using System;
using System.Collections.Generic;
using GenericCellularAutomation.Presentation.Configurations;
using GenericCellularAutomation.Presentation.Drawing;
using GenericCellularAutomation.Presentation.Rules;
using GenericCellularAutomation.Presentation.ViewModels;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.History.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;
using SimulationStorm.ToolPanels.Presentation;

namespace GenericCellularAutomation.Presentation.Startup;

public static class PresentationConfiguration
{
    public const string ApplicationName = "SimulationStorm | Generic Cellular Automation";
    
    public static readonly GcaOptions GcaOptions = new()
    {
        WorldSize = new Size(192, 108),
        WorldSizeRange = new Range<Size>(new Size(1, 1), new Size(1_920, 1_080)),
        WorldWrapping = WorldWrapping.NoWrap,
        MaxCellNeighborhoodRadius = 5,
        Configuration = PredefinedConfigurations.GameOfLife
    };

    public static readonly RulesOptions RulesOptions = new()
    {
        MaxRuleCount = byte.MaxValue,
        MaxRuleSetCount = byte.MaxValue,
        RuleSetRepetitionCountRange = new Range<int>(1, 1000),
        RuleSetCollectionRepetitionCountRange = new Range<int>(1, 1000)
    };
    
    public static readonly SimulationRunnerOptions SimulationRunnerOptions = new()
    {
        MaxStepsPerSecondRange = new Range<int>(1, 100),
        MaxStepsPerSecond = 1
    };
    
    public static readonly ToolPanelOptions ToolPanelOptions = new()
    {
        ToolPanelPositions = new Dictionary<ToolPanel, ToolPanelPosition>
        {
            [GcaToolPanels.SimulationToolPanel] = ToolPanelPosition.TopRight,
            [GcaToolPanels.DrawingToolPanel] = ToolPanelPosition.TopRight,
            [GcaToolPanels.RenderingToolPanel] = ToolPanelPosition.TopLeft,
            [GcaToolPanels.CameraToolPanel] = ToolPanelPosition.TopLeft,
            [GcaToolPanels.HistoryToolPanel] = ToolPanelPosition.BottomRight,
            [GcaToolPanels.StatisticsToolPanel] = ToolPanelPosition.BottomLeft
        },
        ToolPanelVisibilities = new Dictionary<ToolPanel, bool>
        {
            [GcaToolPanels.SimulationToolPanel] = false,
            [GcaToolPanels.DrawingToolPanel] = false,
            [GcaToolPanels.RenderingToolPanel] = false,
            [GcaToolPanels.CameraToolPanel] = false,
            [GcaToolPanels.StatisticsToolPanel] = false,
            [GcaToolPanels.HistoryToolPanel] = false
        },
        ToolPanelViewModelTypes = new Dictionary<ToolPanel, Type>
        {
            [GcaToolPanels.SimulationToolPanel] = typeof(SimulationToolPanelViewModel),
            [GcaToolPanels.DrawingToolPanel] = typeof(DrawingToolPanelViewModel),
            [GcaToolPanels.RenderingToolPanel] = typeof(RenderingToolPanelViewModel),
            [GcaToolPanels.CameraToolPanel] = typeof(CameraSettingsViewModel),
            [GcaToolPanels.StatisticsToolPanel] = typeof(ISummaryStatsViewModel),
            [GcaToolPanels.HistoryToolPanel] = typeof(IHistoryViewModel)
        }
    };
}