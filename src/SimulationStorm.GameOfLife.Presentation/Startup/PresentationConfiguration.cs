using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using SimulationStorm.AppSaves.Persistence;
using SimulationStorm.Collections.StorageControl;
using SimulationStorm.Graphics;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Drawing;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.GameOfLife.Presentation.Population;
using SimulationStorm.GameOfLife.Presentation.Rendering;
using SimulationStorm.GameOfLife.Presentation.Rules;
using SimulationStorm.GameOfLife.Presentation.Stats;
using SimulationStorm.GameOfLife.Presentation.ViewModels;
using SimulationStorm.Logging;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Models;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.StatusBar;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;
using SimulationStorm.ToolPanels.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Startup;

public static class PresentationConfiguration
{
    public const string ApplicationName = "SimulationStorm | Game of Life";
    
    // Directory name is different because vertical separator symbol is not allowed in a directory name.
    public const string ApplicationDirectoryName = "SimulationStorm - Game of Life";

    public static readonly string ApplicationWorkingFilesDirectoryPath = Path.Combine
    (
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        ApplicationDirectoryName
    );

    public static readonly LoggingOptions LoggingOptions = new()
    {
        LogTemplate = "[{Level:u3}] {Timestamp:HH:mm:ss} " +
                      "| Thread: {ThreadId} | {SourceContext} | {Message:lj}{NewLine}{Exception}",

        LogFilesDirectoryPath = Path.Combine(ApplicationWorkingFilesDirectoryPath, "Logs"),
        LogsFileName = $"Logs on {DateTime.Now:dd-MM-yy HH-mm-ss}.txt"
    };

    public static readonly AppSavesOptions AppSavesOptions = new()
    {
        DatabaseDirectoryPath = ApplicationWorkingFilesDirectoryPath,
        DatabaseFileName = "AppStatesDatabase.db",
        AppSaveNameLengthRange = new Range<int>(1, 50)
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

    public static readonly GameOfLifeDrawingOptions DrawingOptions = new()
    {
        IsDrawingEnabled = false,
        BrushRadiusRange = new Range<int>(0, 50),
        BrushRadius = 0,
        BrushShape = DrawingShape.Circle,
        BrushCellState = GameOfLifeCellState.Alive,
        Pattern = null
    };

    public static readonly SimulationRunnerOptions SimulationRunnerOptions = new()
    {
        MaxStepsPerSecondRange = new Range<int>(1, 100),
        MaxStepsPerSecond = 1
    };

    public static readonly PopulationOptions PopulationOptions = new()
    {
        CellBirthProbabilityRange = new Range<double>(0.01, 1),
        CellBirthProbability = 0.01
    };

    public static readonly GameOfLifeRendererOptions GameOfLifeRendererOptions = new()
    {
        IsRenderingEnabled = true,
        RenderingIntervalRange = new Range<int>(0, 1_000),
        RenderingInterval = 0,
        DeadCellColor = KnownColors.White,
        AliveCellColor = KnownColors.Black
    };

    public static readonly CellularWorldRendererOptions CellularWorldRendererOptions = new()
    {
        IsGridLinesVisible = true,
        GridLinesColor = new Color(0, 0, 0, 128),
        HoveredCellColor = new Color(134, 179, 238), // Actipro's ControlBackgroundBrushEmphasizedAccentPointerOver
        PressedCellColor = new Color(88, 147, 166) // Actipro's ControlBackgroundBrushEmphasizedAccentPointerPressed
    };

    public static readonly GameOfLifeManagerOptions GameOfLifeManagerOptions = new()
    {
        // The simulation manager command executed event could be handled by:
        // 1) ISimulationRenderer, 2) IHistoryManager, 3) IStatisticsManager
        CommandExecutedEventHandlerCount = 3,
        
        WorldSizeRange = new Range<Size>(new Size(1, 1), new Size(1_920, 1_080) * 4),
        // WorldSize = new Size(VectorLifeAutomation.BatchSize, VectorLifeAutomation.BatchSize),
        WorldSize = new Size(1_920, 1_080),
        
        Algorithm = GameOfLifeAlgorithm.Bitwise,
        // Algorithm = LifeAutomationAlgorithm.Vector,
        WorldWrapping = WorldWrapping.NoWrap,
        Rule = PredefinedRules.GameOfLife.Rule
    };

    public static readonly HistoryOptions HistoryOptions = new()
    {
        StorageLocation = CollectionStorageLocation.Memory,
        CapacityRange = new Range<int>(1, 1_000),
        Capacity = 50,
        IsSavingEnabled = false,
        SavingIntervalRange = new Range<int>(0, 1_000),
        SavingInterval = 0
    };

    public static readonly StatusBarOptions StatusBarOptions = new()
    {
        IsCommandProgressVisible = true,
        IsCommandTimeVisible = true,
        IsSimulationRenderingProgressVisible = true,
        IsSimulationRenderingTimeVisible = true,
        IsWorldRenderingTimeVisible = true
    };
    
    public static readonly CommandExecutionStatsOptions CommandExecutionStatsOptions = new()
    {
        IsSavingEnabled = false,
        Capacity = 50,
        CapacityRange = new Range<int>(1, 1_000),
        
        ChartTypes = new[]
        {
            ChartType.Line,
            ChartType.Bar,
        },
        DefaultChartType = ChartType.Line,
        
        ViewModelTypesByChartType = new Dictionary<ChartType, Type>
        {
            [ChartType.Line] = typeof(CommandExecutionLineChartViewModel),
            [ChartType.Bar] = typeof(CommandExecutionBarChartViewModel),
        },
        
        CommandNumberAxisNameKey = "Simulation.Stats.CommandNumber",
        ExecutionTimeAxisNameKey = "Simulation.Stats.ExecutionTime",
        ExecutionTimeAxisTimeUnit = TimeSpan.FromMicroseconds(1)
    };
    
    public static readonly RenderingStatsOptions RenderingStatsOptions = new()
    {
        IsSavingEnabled = false,
        Capacity = 50,
        CapacityRange = new Range<int>(1, 1_000),
        
        ChartTypes = new[]
        {
            ChartType.Line,
            ChartType.Bar,
        },
        DefaultChartType = ChartType.Line,
        
        ViewModelTypesByChartType = new Dictionary<ChartType, Type>
        {
            [ChartType.Line] = typeof(RenderingLineChartViewModel),
            [ChartType.Bar] = typeof(RenderingBarChartViewModel),
        },
        
        CommandNumberAxisNameKey = "Simulation.Stats.CommandNumber",
        RenderingTimeAxisNameKey = "Simulation.Stats.RenderingTime",
        RenderingTimeAxisTimeUnit = TimeSpan.FromMicroseconds(1)
    };
    
    public static readonly SummaryStatsOptions SummaryStatsOptions = new()
    {
        StorageLocation = CollectionStorageLocation.Memory,
        CapacityRange = new Range<int>(1, 1_000),
        Capacity = 50,
        IsSavingEnabled = false,
        SavingIntervalRange = new Range<int>(0, 1_000),
        SavingInterval = 0,
        
        ChartTypes = new[]
        {
            ChartType.Table,
            ChartType.Pie,
            ChartType.Bar,
            ChartType.Line
        },
        DefaultChartType = ChartType.Table,
        
        ViewModelTypesByChartType = new Dictionary<ChartType, Type>
        {
            [ChartType.Table] = typeof(TableChartViewModel),
            [ChartType.Pie] = typeof(PieChartViewModel),
            [ChartType.Bar] = typeof(BarChartViewModel),
            [ChartType.Line] = typeof(LineChartViewModel)
        },
        
        CommandNumberAxisNameKey = "Simulation.Stats.CommandNumber",
        AliveCellCountAxisNameKey = "GameOfLife.AliveCellCount",
        DeadCellsStringKey = "GameOfLife.DeadCells",
        AliveCellsStringKey = "GameOfLife.AliveCells"
    };

    public static readonly ToolPanelOptions ToolPanelOptions = new()
    {
        ToolPanelPositions = new Dictionary<ToolPanel, ToolPanelPosition>
        {
            [GameOfLifeToolPanels.SimulationToolPanel] = ToolPanelPosition.TopRight,
            [GameOfLifeToolPanels.DrawingToolPanel] = ToolPanelPosition.TopRight,
            [GameOfLifeToolPanels.RenderingToolPanel] = ToolPanelPosition.TopLeft,
            [GameOfLifeToolPanels.CameraToolPanel] = ToolPanelPosition.TopLeft,
            [GameOfLifeToolPanels.HistoryToolPanel] = ToolPanelPosition.BottomRight,
            [GameOfLifeToolPanels.StatisticsToolPanel] = ToolPanelPosition.BottomLeft
        },
        ToolPanelVisibilities = new Dictionary<ToolPanel, bool>
        {
            [GameOfLifeToolPanels.SimulationToolPanel] = false,
            [GameOfLifeToolPanels.DrawingToolPanel] = false,
            [GameOfLifeToolPanels.RenderingToolPanel] = false,
            [GameOfLifeToolPanels.CameraToolPanel] = false,
            [GameOfLifeToolPanels.StatisticsToolPanel] = false,
            [GameOfLifeToolPanels.HistoryToolPanel] = false
        },
        ToolPanelViewModelTypes = new Dictionary<ToolPanel, Type>
        {
            [GameOfLifeToolPanels.SimulationToolPanel] = typeof(SimulationToolPanelViewModel),
            [GameOfLifeToolPanels.DrawingToolPanel] = typeof(DrawingToolPanelViewModel),
            [GameOfLifeToolPanels.RenderingToolPanel] = typeof(RenderingToolPanelViewModel),
            [GameOfLifeToolPanels.CameraToolPanel] = typeof(CameraSettingsViewModel),
            [GameOfLifeToolPanels.StatisticsToolPanel] = typeof(ISummaryStatsViewModel),
            [GameOfLifeToolPanels.HistoryToolPanel] = typeof(IHistoryViewModel)
        }
    };
}