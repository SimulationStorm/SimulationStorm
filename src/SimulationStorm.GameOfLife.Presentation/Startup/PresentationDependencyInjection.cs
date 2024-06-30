using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Presentation;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.Universal;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Exceptions;
using SimulationStorm.Exceptions.Logging;
using SimulationStorm.Graphics;
using SimulationStorm.Graphics.Skia;
using SimulationStorm.GameOfLife.Presentation.Drawing;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.GameOfLife.Presentation.Patterns;
using SimulationStorm.GameOfLife.Presentation.Population;
using SimulationStorm.GameOfLife.Presentation.Rendering;
using SimulationStorm.GameOfLife.Presentation.Rules;
using SimulationStorm.GameOfLife.Presentation.ViewModels;
using SimulationStorm.Logging;
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
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Simulation.Resetting.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.ToolPanels.Presentation;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;
using System;
using LiveChartsCore.Kernel.Sketches;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Stats;
using SimulationStorm.Presentation.StartupOperations;
using SimulationStorm.Simulation.Presentation.StatusBar;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;

namespace SimulationStorm.GameOfLife.Presentation.Startup;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services) => services
        // Independent transient services
        .AddTransient<IBenchmarkingService, BenchmarkingService>()
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
        .AddAsyncServiceStateManager<GameOfLifeStateManager>()
        //

        // Simulation runner
        .AddSingleton(PresentationConfiguration.SimulationRunnerOptions)
        .AddSingleton<ISimulationRunner, SimulationRunner>()
        .AddSingleton<SimulationRunnerViewModel>()
        .AddServiceStateManager<SimulationRunnerStateManager>()
        .AddAppStateRestoringOperation<PauseSimulationOnAppStateRestoringOperation>()
        //
        
        // Tool panels infrastructure
        .AddSingleton(PresentationConfiguration.ToolPanelOptions)
        .AddSingleton<IToolPanelManager, ToolPanelManager>()
        .AddSingleton<IToolPanelViewModelFactory, ToolPanelViewModelFactory>()
        .AddSingleton<ToolPanelManagerViewModel>()
        .AddServiceStateManager<ToolPanelStatesManager>()
        //
        
        // Simulation and world rendering
        // Simulation renderer
        .AddSingleton(PresentationConfiguration.GameOfLifeRendererOptions)
        .AddSharedSingleton<ISimulationRendererOptions, GameOfLifeRendererOptions>()
        .AddSingleton<GameOfLifeRenderer>()
        .AddSharedSingleton<ISimulationRenderer, GameOfLifeRenderer>()
        .AddServiceStateManager<GameOfLifeRendererStateManager>()
        .AddStartupOperation<RenderSimulationOnStartupOperation>()
        //
        .AddSingleton<IWorldViewport, WorldViewport>()
        // Camera
        .AddSingleton(PresentationConfiguration.WorldCameraOptions)
        .AddSingleton<IWorldCamera, WorldCamera>()
        .AddSingleton<CameraSettingsViewModel>()
        .AddServiceStateManager<WorldCameraStateManager>()
        //
        // World renderer
        .AddSingleton<ICellularWorldRendererOptions>(PresentationConfiguration.CellularWorldRendererOptions)
        .AddSingleton<BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IBoundedWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<ICellularWorldRenderer, BoundedCellularWorldRenderer>()
        .AddSharedSingleton<IBoundedCellularWorldRenderer, BoundedCellularWorldRenderer>()
        .AddServiceStateManager<BoundedCellularWorldRendererStateManager>()
        //
        .AddSingleton<GameOfLifeWorldViewModel>()
        //

        // Drawing on world
        .AddSingleton(PresentationConfiguration.DrawingOptions)
        .AddSharedSingleton<IDrawingOptions<GameOfLifeCellState>, GameOfLifeDrawingOptions>()
        
        .AddSingleton<GameOfLifeDrawingSettings>()
        .AddSharedSingleton<IDrawingSettings<GameOfLifeCellState>, GameOfLifeDrawingSettings>()
        .AddServiceStateManager<GameOfLifeDrawingSettingsStateManager>()
        
        .AddSingleton<IDrawingSettingsViewModel, DrawingSettingsViewModel>()
        .AddSingleton<DrawingToolPanelViewModel>()
        //

        // History
        .AddSingleton(PresentationConfiguration.HistoryOptions)
        .AddSingleton<IHistoryManager<GameOfLifeSave>, HistoryManager<GameOfLifeSave>>()
        .AddServiceStateManager<HistoryStateManager<GameOfLifeSave>>()
        .AddSingleton<IHistoryViewModel, HistoryViewModel<GameOfLifeSave>>()
        //

        // Statistics
        // Summary statistics
        .AddSingleton(PresentationConfiguration.SummaryStatsOptions)
        .AddSharedSingleton<ISummaryStatsOptions, SummaryStatsOptions>()
        .AddSingleton<ISummaryStatsManager<GameOfLifeSummary>, SummaryStatsManager<GameOfLifeSummary>>()
        // .AddSingleton<ISummaryStatsManager<GameOfLifeSummary>, HistoryAwareSummaryStatsManager<GameOfLifeSummary, GameOfLifeSave>>()
        .AddServiceStateManager<SummaryStatsStateManager<GameOfLifeSummary>>()
        .AddSingleton<ISummaryStatsViewModel, SummaryStatsViewModel<GameOfLifeSummary>>()
        .AddServiceStateManager<SummaryStatsViewModelStateManager>()
        // Summary statistics chart view models
        .AddSingleton<SummaryStatsChartViewModelFactory>()
        .AddTransient<TableChartViewModel>()
        .AddTransient<PieChartViewModel>()
        .AddTransient<LineChartViewModel>()
        .AddTransient<BarChartViewModel>()
        //
        // Command execution statistics
        .AddSingleton(PresentationConfiguration.CommandExecutionStatsOptions)
        .AddSingleton<ICommandExecutionStatsManager, CommandExecutionStatsManager>()
        .AddServiceStateManager<CommandExecutionStatsStateManager>()
        .AddSingleton<CommandExecutionStatsViewModel>()
        .AddServiceStateManager<CommandExecutionStatsViewModelStateManager>()
        
        .AddSingleton<CommandExecutionChartViewModelFactory>()
        .AddTransient<CommandExecutionLineChartViewModel>()
        .AddTransient<CommandExecutionBarChartViewModel>()
        //
        // Simulation rendering statistics
        .AddSingleton(PresentationConfiguration.RenderingStatsOptions)
        .AddSingleton<IRenderingStatsManager, RenderingStatsManager>()
        .AddServiceStateManager<RenderingStatsStateManager>()
        .AddSingleton<RenderingStatsViewModel>()
        .AddServiceStateManager<RenderingStatsViewModelStateManager>()

        .AddSingleton<RenderingChartViewModelFactory>()
        .AddTransient<RenderingLineChartViewModel>()
        .AddTransient<RenderingBarChartViewModel>()
        //

        // Simulation tool panel
        .AddSingleton<SimulationToolPanelViewModel>()
        .AddSingleton<IWorldSizeViewModel, WorldSizeViewModel>()
        .AddServiceStateManager<WorldSizeViewModelStateManager>()
        .AddSingleton<IWorldWrappingViewModel, WorldWrappingViewModel<GameOfLifeCellState>>()
        // Population
        .AddSingleton(PresentationConfiguration.PopulationOptions)
        .AddSingleton<PopulationViewModel>()
        .AddServiceStateManager<PopulationViewModelStateManager>()
        //
        .AddSingleton<AlgorithmViewModel>()
        .AddSingleton<PatternsViewModel>()
        .AddSingleton<RuleViewModel>()
        .AddSingleton<CommandQueueViewModel>()
        .AddServiceStateManager<CommandQueueViewModelStateManager>()
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
        .AddServiceStateManager<StatusBarStateManager>()
        //
    
        .AddAppStateManagementServices(PresentationConfiguration.AppStatesOptions)
    
        .AddStartupOperationManager();
}