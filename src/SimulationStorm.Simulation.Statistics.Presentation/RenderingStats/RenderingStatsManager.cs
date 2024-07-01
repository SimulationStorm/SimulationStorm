using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationRenderer;
using SimulationStorm.Simulation.Statistics.Presentation.RenderingStats.DataTypes;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.Statistics.Presentation.RenderingStats;

public class RenderingStatsManager : CollectionManagerBase<RenderingResultRecord>, IRenderingStatsManager
{
    public RenderingStatsManager
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ISimulationRenderer simulationRenderer,
        RenderingStatsOptions options
    )
        : base(universalCollectionFactory, intervalActionExecutor, options)
    {
        Observable
            .FromEventPattern<EventHandler<SimulationRenderingCompletedEventArgs>, SimulationRenderingCompletedEventArgs>
            (
                h => simulationRenderer.RenderingCompleted += h,
                h => simulationRenderer.RenderingCompleted -= h
            )
            .Where(_ => IsSavingEnabled)
            .Select(e => e.EventArgs)
            .Where(e => e.Command is not RestoreStateCommand { IsRestoringFromAppSave: true })
            .Select(e => new RenderingResultRecord(e.Command, e.ElapsedTime))
            .Subscribe(Collection.Add)
            .DisposeWith(Disposables);
    }
}