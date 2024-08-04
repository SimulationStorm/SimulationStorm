using GenericCellularAutomation.Presentation.Management;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation;
using SimulationStorm.Collections.Lists;
using SimulationStorm.Collections.Universal;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Graphics;
using SimulationStorm.Graphics.Skia;
using SimulationStorm.Presentation.StartupOperations;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.Presentation.SimulationManager;
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
        .AddServiceSaveManager<ToolPanelsSaveManager>()
    //
        ;
}