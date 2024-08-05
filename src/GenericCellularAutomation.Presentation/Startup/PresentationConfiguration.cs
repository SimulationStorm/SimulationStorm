using System;
using System.Collections.Generic;
using System.Numerics;
using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Configurations;
using GenericCellularAutomation.Presentation.Drawing;
using GenericCellularAutomation.Presentation.Rendering;
using GenericCellularAutomation.Presentation.Rules;
using GenericCellularAutomation.Presentation.ViewModels;
using SimulationStorm.Graphics;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
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
    
    // Todo: how to name? Gca or Simulation renderer options?
    public static readonly SimulationRendererOptions GcaRendererOptions = new()
    {
        IsRenderingEnabled = true,
        RenderingIntervalRange = new Range<int>(0, 1_000),
        RenderingInterval = 0,
    };

    public static readonly CellularWorldRendererOptions CellularWorldRendererOptions = new()
    {
        IsGridLinesVisible = true,
        GridLinesColor = new Color(0, 0, 0, 128),
        HoveredCellColor = new Color(134, 179, 238), // Actipro's ControlBackgroundBrushEmphasizedAccentPointerOver
        PressedCellColor = new Color(88, 147, 166) // Actipro's ControlBackgroundBrushEmphasizedAccentPointerPressed
    };
    
    public static readonly WorldCameraOptions WorldCameraOptions = new()
    {
        ZoomRange = new Range<float>(0.1f, 100f),
        ZoomChangeRange = new Range<float>(0.01f, 10f),
        DefaultZoomChange = 0.25f,
        DefaultZoom = 10f,
        TranslationRange = new Range<PointF>(new PointF(-100_000, -100_000), new PointF(100_000, 100_000)),
        TranslationChangeRange = new Range<float>(1, 1_000),
        DefaultTranslationChange = 25f,
        DefaultTranslation = Vector2.Zero
    };
}