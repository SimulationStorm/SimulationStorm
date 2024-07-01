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

    private readonly AsyncManualResetEvent _advancementLoopSynchronizer = new(false);
    
    private readonly CancellationTokenSource _advancementLoopCts = new();

    private Task _advancementLoopTask = null!;

    private readonly Stopwatch _advancementLoopStopwatch = new();
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
        
        StartSimulationAdvancementLoop();
    }

    #region Public methods
    public void StartSimulation()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        if (SimulationRunningState is SimulationRunningState.Running)
            return;

        _advancementLoopSynchronizer.Set();
        
        SimulationRunningState = SimulationRunningState.Running;
        
        _notificationManager.ShowInformation("Simulation.Running.AdvancementStartedMessage", "Notifications.Notification");
    }

    public void PauseSimulation()
    {
        this.ThrowIfDisposingOrDisposed(IsDisposingOrDisposed);
        
        if (SimulationRunningState is SimulationRunningState.Paused)
            return;

        _advancementLoopSynchronizer.Reset();

        SimulationRunningState = SimulationRunningState.Paused;
        
        _notificationManager.ShowInformation("Simulation.Running.AdvancementPausedMessage", "Notifications.Notification");
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _advancementLoopCts
            .CancelAsync()
            .ConfigureAwait(false);
        
        await _advancementLoopTask
            .ConfigureAwait(false);
        
        await _advancementLoopSynchronizer
            .DisposeAsync()
            .ConfigureAwait(false);
        
        _advancementLoopCts.Dispose();
    }
    #endregion

    #region Simulation advancement cycle
    private void StartSimulationAdvancementLoop() =>
        _advancementLoopTask = AdvanceSimulationInLoopAsync(_advancementLoopCts.Token);

    private async Task AdvanceSimulationInLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                await _advancementLoopSynchronizer
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);

                _advancementLoopStopwatch.Restart();
                await _simulationManager
                    .AdvanceAsync()
                    .ConfigureAwait(false);
                _advancementLoopStopwatch.Stop();
                
                NotifySimulationAdvanced(_advancementLoopStopwatch.Elapsed);

                var elapsedTimeInMs = (int)_advancementLoopStopwatch.ElapsedMilliseconds;
                if (elapsedTimeInMs >= 1000 / MaxStepsPerSecond)
                    continue;

                var delayTimeInMs = 1000 / MaxStepsPerSecond - elapsedTimeInMs;
                await Task
                    .Delay(delayTimeInMs, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) { }
    }

    private void NotifySimulationAdvanced(TimeSpan elapsedTime) =>
        SimulationAdvanced?.Invoke(this, new SimulationAdvancedEventArgs(elapsedTime));
    #endregion
}