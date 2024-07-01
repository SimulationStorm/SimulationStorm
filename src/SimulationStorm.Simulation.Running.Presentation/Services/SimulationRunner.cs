using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DotNext.Threading;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Simulation.Running.Presentation.Models;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Running.Presentation.Services;

public partial class SimulationRunner : AsyncDisposableObservableObject, ISimulationRunner
{
    #region Properties
    [ObservableProperty] private SimulationRunningState _simulationRunningState;

    [ObservableProperty] private int _maxStepsPerSecond;
    #endregion

    public event EventHandler<SimulationAdvancedEventArgs>? SimulationAdvanced;

    #region Fields
    private readonly IAdvanceableSimulationManager _simulationManager;

    private readonly ILocalizedNotificationManager _notificationManager;

    private readonly AsyncManualResetEvent _advancingCycleSynchronizer = new(false);
    
    private readonly CancellationTokenSource _advancingCycleCts = new();

    private readonly Task _advancingCycleTask;

    private readonly Stopwatch _advancingCycleStopwatch = new();
    #endregion

    public SimulationRunner
    (
        IAdvanceableSimulationManager simulationManager,
        ILocalizedNotificationManager notificationManager,
        SimulationRunnerOptions options)
    {
        _simulationManager = simulationManager;
        _notificationManager = notificationManager;
        
        MaxStepsPerSecond = options.MaxStepsPerSecond;

        _advancingCycleTask = AdvanceSimulationInCycleAsync(_advancingCycleCts.Token);
    }

    #region Public methods
    public void StartSimulation()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        if (SimulationRunningState is SimulationRunningState.Running)
            return;

        StartAdvancementCycle();
        
        SimulationRunningState = SimulationRunningState.Running;
        
        _notificationManager.ShowInformation("Simulation.Running.AdvancementStartedMessage", "Notifications.Notification");
    }

    public void PauseSimulation()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        if (SimulationRunningState is SimulationRunningState.Paused)
            return;

        PauseAdvancementCycle();

        SimulationRunningState = SimulationRunningState.Paused;
        
        _notificationManager.ShowInformation("Simulation.Running.AdvancementPausedMessage", "Notifications.Notification");
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _advancingCycleCts
            .CancelAsync()
            .ConfigureAwait(false);
        
        await _advancingCycleTask
            .ConfigureAwait(false);
        
        await _advancingCycleSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
        
        _advancingCycleCts.Dispose();
    }
    #endregion

    #region Simulation advancement cycle
    private void StartAdvancementCycle() => _advancingCycleSynchronizer.Set();

    private void PauseAdvancementCycle() => _advancingCycleSynchronizer.Reset();
    
    private async Task AdvanceSimulationInCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                await _advancingCycleSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);

                _advancingCycleStopwatch.Restart();
                await _simulationManager
                    .AdvanceAsync()
                    .ConfigureAwait(false);
                _advancingCycleStopwatch.Stop();
                
                NotifySimulationAdvanced(_advancingCycleStopwatch.Elapsed);

                var elapsedTimeInMs = (int)_advancingCycleStopwatch.ElapsedMilliseconds;
                if (elapsedTimeInMs >= 1000 / MaxStepsPerSecond)
                    continue;

                var delayTimeInMs = 1000 / MaxStepsPerSecond - elapsedTimeInMs;
                await Task
                    .Delay(delayTimeInMs, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException _)
        {
            // Do nothing
        }
    }

    private void NotifySimulationAdvanced(TimeSpan elapsedTime) =>
        SimulationAdvanced?.Invoke(this, new SimulationAdvancedEventArgs(elapsedTime));
    #endregion
}