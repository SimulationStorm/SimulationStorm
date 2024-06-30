using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats.DataTypes;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.Statistics.Presentation.CommandExecutionStats;

public class CommandExecutionStatsManager : CollectionManagerBase<CommandExecutionResultRecord>, ICommandExecutionStatsManager
{
    public CommandExecutionStatsManager
    (
        IUniversalCollectionFactory universalCollectionFactory,
        IIntervalActionExecutor intervalActionExecutor,
        ISimulationManager simulationManager,
        CommandExecutionStatsOptions options
    )
        : base(universalCollectionFactory, intervalActionExecutor, options)
    {
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
                (
                    h => simulationManager.CommandCompleted += h,
                    h => simulationManager.CommandCompleted -= h
                )
                .Where(_ => IsSavingEnabled)
                .Select(e => e.EventArgs)
                .Where(e => e.Command is not RestoreStateCommand { IsRestoringFromAppState: true })
                .Select(e => new CommandExecutionResultRecord(e.Command, e.ElapsedTime))
                .Subscribe(Collection.Add)
                .DisposeWith(disposables);
        });
    }
}