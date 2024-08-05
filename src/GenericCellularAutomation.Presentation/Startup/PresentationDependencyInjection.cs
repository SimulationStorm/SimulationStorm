using GenericCellularAutomation.Presentation.Common;
using GenericCellularAutomation.Presentation.Management;
using GenericCellularAutomation.Presentation.Rendering;
using GenericCellularAutomation.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves;
using SimulationStorm.AppSaves.Presentation;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.Universal;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Graphics;
using SimulationStorm.Graphics.Skia;
using SimulationStorm.Presentation.StartupOperations;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.Cellular.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.Presentation.Camera;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Simulation.Presentation.Viewport;
using SimulationStorm.Simulation.Presentation.WorldRenderer;
using SimulationStorm.Simulation.Resetting.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.ViewModels;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.ToolPanels.Presentation;
using SimulationStorm.Utilities;
using SimulationStorm.Utilities.Benchmarking;

namespace GenericCellularAutomation.Presentation.Startup;

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
        .AddSingleton<IGcaFactory, GcaFactory>()
    //
    
    // Simulation manager
        .AddSingleton(PresentationConfiguration.GcaOptions)
        .AddSharedSingleton<IBoundedSimulationManagerOptions, GcaOptions>()
        
        .AddSingleton<GcaManager>()
        .AddSharedSingleton<ISimulationManager, GcaManager>()
        .AddSharedSingleton<IAdvanceableSimulationManager, GcaManager>()
        .AddSharedSingleton<IResettableSimulationManager, GcaManager>()
        .AddSharedSingleton<IBoundedSimulationManager, GcaManager>()
        .AddSharedSingleton<ISaveableSimulationManager<GcaSave>, GcaManager>()
        .AddSharedSingleton<ISummarizableSimulationManager<GcaSummary>, GcaManager>()
        .AddSharedSingleton<ICellularAutomationManager<GcaCellState>, GcaManager>()
        .AddSharedSingleton<IBoundedCellularAutomationManager<GcaCellState>, GcaManager>()
        .AddAsyncServiceSaveManager<GcaSaveManager>()
        
        .AddStartupOperation<AddSimulationCommandCompletedHandlersOnStartupOperation>()
    //
        .AddSimulationRunner(PresentationConfiguration.SimulationRunnerOptions)
        .AddToolPanels(PresentationConfiguration.ToolPanelOptions)
        
        // Simulation and world rendering
        // Simulation renderer
        .AddSimulationRenderer<SimulationRendererOptions, GcaRenderer, SimulationRendererSaveManager>(
            PresentationConfiguration.GcaRendererOptions)
        
        // World viewport
        .AddSingleton<IWorldViewport, WorldViewport>()
        // Camera
        .AddWorldCamera(PresentationConfiguration.WorldCameraOptions)
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
        .AddSingleton<GcaWorldViewModel>()
    //
    ;

    private static IServiceCollection AddSimulationRunner
    (
        this IServiceCollection services,
        SimulationRunnerOptions options)
    {
        return services
            .AddSingleton(options)
            .AddSingleton<ISimulationRunner, SimulationRunner>()
            .AddSingleton<SimulationRunnerViewModel>()
            .AddServiceSaveManager<SimulationRunnerSaveManager>()
            .AddAppSaveRestoringOperation<PauseSimulationOnAppSaveRestoringOperation>();
    }

    private static IServiceCollection AddWorldCamera
    (
        this IServiceCollection services,
        WorldCameraOptions options)
    {
        return services
            .AddSingleton(options)
            .AddSingleton<IWorldCamera, WorldCamera>()
            .AddSingleton<CameraSettingsViewModel>()
            .AddServiceSaveManager<WorldCameraSaveManager>();
    }
    
    private static IServiceCollection AddSimulationRenderer<TOptions, TRenderer, TSaveManager>
    (
        this IServiceCollection services,
        TOptions options
    )
        where TOptions : class, ISimulationRendererOptions
        where TRenderer : class, ISimulationRenderer
        where TSaveManager : class, IServiceSaveManager
    {
        return services
            .AddSingleton(options)
            .AddSharedSingleton<ISimulationRendererOptions, TOptions>()
            .AddSingleton<TRenderer>()
            .AddSharedSingleton<ISimulationRenderer, TRenderer>()
            .AddSharedSingleton<ISimulationCommandCompletedHandler, ISimulationRenderer>()
            .AddServiceSaveManager<TSaveManager>()
            .AddStartupOperation<RenderSimulationOnStartupOperation>();
    }
    
    private static IServiceCollection AddToolPanels(this IServiceCollection services, ToolPanelOptions options) => services
        .AddSingleton(PresentationConfiguration.ToolPanelOptions)
        .AddSingleton<IToolPanelManager, ToolPanelManager>()
        .AddSingleton<IToolPanelViewModelFactory, ToolPanelViewModelFactory>()
        .AddSingleton<ToolPanelManagerViewModel>()
        .AddServiceSaveManager<ToolPanelsSaveManager>();
}