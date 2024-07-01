using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.Universal;
using SimulationStorm.DependencyInjection;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Drawing;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.GameOfLife.Presentation.Patterns;
using SimulationStorm.GameOfLife.Presentation.Population;
using SimulationStorm.GameOfLife.Presentation.Rendering;
using SimulationStorm.GameOfLife.Presentation.Rules;
using SimulationStorm.GameOfLife.Presentation.Stats;
using SimulationStorm.GameOfLife.Presentation.ViewModels;
using SimulationStorm.Graphics;
using SimulationStorm.Graphics.Skia;
using SimulationStorm.Presentation.StartupOperations;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Bounded.Presentation.ViewModels;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.Cellular.Presentation.ViewModels;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.ViewModels;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Simulation.Presentation.StatusBar;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Simulation.Resetting.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;
using SimulationStorm.ToolPanels.Presentation;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.GameOfLife.Presentation.Startup;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services) => services
        // Independent transient services
        .AddTransient<IBenchmarker, StopwatchBenchmarker>()
        .AddTransient<IIntervalActionExecutor, IntervalActionExecutor>()
        //
        
        // Independent singleton services
        .AddSingleton<IGraphicsFactory, SkiaGraphicsFactory>()
        .AddSingleton<IListFactory, ListFactory>()
        .AddSingleton<IUniversalCollectionFactory, UniversalCollectionFactory>()
        .AddSingleton<IGameOfLifeFactory, GameOfLifeFactory>()
        //
        
        // Simulation manager
        .AddSingleton(PresentationConfiguration.GameOfLifeManagerOptions)
        .AddSharedSingleton<IBoundedSimulationManagerOptions, GameOfLifeManagerOptions>()
        
        .AddSingleton<GameOfLifeManager>()
        .AddSharedSingleton<ISimulationManager, GameOfLifeManager>()
        .AddSharedSingleton<IAdvanceableSimulationManager, GameOfLifeManager>()
        .AddSharedSingleton<IResettableSimulationManager, GameOfLifeManager>()
        .AddSharedSingleton<IBoundedSimulationManager, GameOfLifeManager>()
        .AddSharedSingleton<ISaveableSimulationManager<GameOfLifeSave>, GameOfLifeManager>()
        .AddSharedSingleton<ISummarizableSimulationManager<GameOfLifeSummary>, GameOfLifeManager>()
        .AddSharedSingleton<ICellularAutomationManager<GameOfLifeCellState>, GameOfLifeManager>()
        .AddSharedSingleton<IBoundedCellularAutomationManager<GameOfLifeCellState>, GameOfLifeManager>()
        .AddAsyncServiceSaveManager<GameOfLifeSaveManager>()
        
        .AddStartupOperation<AddSimulationCommandCompletedHandlersOnStartupOperation>()
        //

        // Simulation runner
        .AddSingleton(PresentationConfiguration.SimulationRunnerOptions)
        .AddSingleton<ISimulationRunner, SimulationRunner>()
        .AddSingleton<SimulationRunnerViewModel>()
        .AddServiceSaveManager<SimulationRunnerSaveManager>()
        .AddAppSaveRestoringOperation<PauseSimulationOnAppSaveRestoringOperation>()
        //
        
        // Tool panels infrastructure
        .AddSingleton(PresentationConfiguration.ToolPanelOptions)
        .AddSingleton<IToolPanelManager, ToolPanelManager>()
        .AddSingleton<IToolPanelViewModelFactory, ToolPanelViewModelFactory>()
        .AddSingleton<ToolPanelManagerViewModel>()
        .AddServiceSaveManager<ToolPanelSavesManager>()
        //
        
        // Simulation and world rendering
        // Simulation renderer
        .AddSingleton(PresentationConfiguration.GameOfLifeRendererOptions)
        .AddSharedSingleton<ISimulationRendererOptions, GameOfLifeRendererOptions>()
        .AddSingleton<GameOfLifeRenderer>()
        .AddSharedSingleton<ISimulationRenderer, GameOfLifeRenderer>()
        .AddSharedSingleton<ISimulationCommandCompletedHandler, ISimulationRenderer>()
        .AddServiceSaveManager<GameOfLifeRendererSaveManager>()
        
        .AddStartupOperation<RenderSimulationOnStartupOperation>()
        //
        .AddSingleton<IWorldViewport, WorldViewport>()
        // Camera
        .AddSingleton(PresentationConfiguration.WorldCameraOptions)
        .AddSingleton<IWorldCamera, WorldCamera>()
        .AddSingleton<CameraSettingsViewModel>()
        .AddServiceSaveManager<WorldCameraSaveManager>()
        //
        // World renderer
        .AddSingleton<ICellularWorldRendererOptions>(PresentationConfiguration.CellularWorldRendererOptions)
        .AddSingleton<BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IBoundedWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<ICellularWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IBoundedCellularWorldRenderer, BoundedCellularWorldRenderer>()
        .AddServiceSaveManager<BoundedCellularWorldRendererSaveManager>()
        //
        .AddSingleton<GameOfLifeWorldViewModel>()
        //

        // Drawing on world
        .AddSingleton(PresentationConfiguration.DrawingOptions)
        .AddSharedSingleton<IDrawingOptions<GameOfLifeCellState>, GameOfLifeDrawingOptions>()
        
        .AddSingleton<GameOfLifeDrawingSettings>()
        .AddSharedSingleton<IDrawingSettings<GameOfLifeCellState>, GameOfLifeDrawingSettings>()
        .AddServiceSaveManager<GameOfLifeDrawingSettingsSaveManager>()
        
        .AddSingleton<IDrawingSettingsViewModel, DrawingSettingsViewModel>()
        .AddSingleton<DrawingToolPanelViewModel>()
        //

        // History
        .AddSingleton(PresentationConfiguration.HistoryOptions)
        .AddSingleton<IHistoryManager<GameOfLifeSave>, HistoryManager<GameOfLifeSave>>()
        .AddSharedSingleton<ISimulationCommandCompletedHandler, IHistoryManager<GameOfLifeSave>>()
        .AddServiceSaveManager<HistorySaveManager<GameOfLifeSave>>()
        .AddSingleton<IHistoryViewModel, HistoryViewModel<GameOfLifeSave>>()
        //

        // Stats
        // Summary stats
        .AddSingleton(PresentationConfiguration.SummaryStatsOptions)
        .AddSharedSingleton<ISummaryStatsOptions, SummaryStatsOptions>()
        .AddSingleton<ISummaryStatsManager<GameOfLifeSummary>, SummaryStatsManager<GameOfLifeSummary>>()
        .AddSharedSingleton<ISimulationCommandCompletedHandler, ISummaryStatsManager<GameOfLifeSummary>>()
        // .AddSingleton<ISummaryStatsManager<GameOfLifeSummary>, HistoryAwareSummaryStatsManager<GameOfLifeSummary, GameOfLifeSave>>()
        .AddServiceSaveManager<SummaryStatsSaveManager<GameOfLifeSummary>>()
        .AddSingleton<ISummaryStatsViewModel, SummaryStatsViewModel<GameOfLifeSummary>>()
        .AddServiceSaveManager<SummaryStatsViewModelSaveManager>()
        // Summary stats chart view models
        .AddSingleton<SummaryStatsChartViewModelFactory>()
        .AddTransient<TableChartViewModel>()
        .AddTransient<PieChartViewModel>()
        .AddTransient<LineChartViewModel>()
        .AddTransient<BarChartViewModel>()
        //
        // Command execution stats
        .AddSingleton(PresentationConfiguration.CommandExecutionStatsOptions)
        .AddSingleton<ICommandExecutionStatsManager, CommandExecutionStatsManager>()
        .AddServiceSaveManager<CommandExecutionStatsSaveManager>()
        .AddSingleton<CommandExecutionStatsViewModel>()
        .AddServiceSaveManager<CommandExecutionStatsViewModelSaveManager>()
        
        .AddSingleton<CommandExecutionChartViewModelFactory>()
        .AddTransient<CommandExecutionLineChartViewModel>()
        .AddTransient<CommandExecutionBarChartViewModel>()
        //
        // Simulation rendering stats
        .AddSingleton(PresentationConfiguration.RenderingStatsOptions)
        .AddSingleton<IRenderingStatsManager, RenderingStatsManager>()
        .AddServiceSaveManager<RenderingStatsSaveManager>()
        .AddSingleton<RenderingStatsViewModel>()
        .AddServiceSaveManager<RenderingStatsViewModelSaveManager>()

        .AddSingleton<RenderingChartViewModelFactory>()
        .AddTransient<RenderingLineChartViewModel>()
        .AddTransient<RenderingBarChartViewModel>()
        //

        // Simulation tool panel
        .AddSingleton<SimulationToolPanelViewModel>()
        .AddSingleton<IWorldSizeViewModel, WorldSizeViewModel>()
        .AddServiceSaveManager<WorldSizeViewModelSaveManager>()
        .AddSingleton<IWorldWrappingViewModel, WorldWrappingViewModel<GameOfLifeCellState>>()
        // Population
        .AddSingleton(PresentationConfiguration.PopulationOptions)
        .AddSingleton<PopulationViewModel>()
        .AddServiceSaveManager<PopulationViewModelSaveManager>()
        //
        .AddSingleton<AlgorithmViewModel>()
        .AddSingleton<PatternsViewModel>()
        .AddSingleton<RuleViewModel>()
        .AddSingleton<ScheduledCommandsViewModel>()
        .AddServiceSaveManager<ScheduledCommandsViewModelSaveManager>()
        //

        // Rendering tool panel
        .AddSingleton<RenderingToolPanelViewModel>()
        .AddSingleton<SimulationRenderingSettingsViewModel>()
        .AddSingleton<CellColorsViewModel>()
        .AddSingleton<WorldBackgroundColorViewModel>()
        .AddSingleton<WorldGridLinesViewModel>()
        //
        
        // Status bar
        .AddSingleton(PresentationConfiguration.StatusBarOptions)
        .AddSingleton<StatusBarViewModel>()
        .AddServiceSaveManager<StatusBarSaveManager>()
        //
    
        .AddAppSaveManagementServices(PresentationConfiguration.AppSavesOptions)
    
        .AddStartupOperationManager();
}