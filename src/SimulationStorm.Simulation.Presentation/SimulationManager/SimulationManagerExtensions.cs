using System;
using System.Reactive.Linq;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public static class SimulationManagerExtensions
{
    public static IObservable<SimulationCommandEventArgs> CommandScheduledObservable
    (
        this ISimulationManager simulationManager)
    {
        return Observable
            .FromEventPattern<EventHandler<SimulationCommandEventArgs>, SimulationCommandEventArgs>
            (
                h => simulationManager.CommandScheduled += h,
                h => simulationManager.CommandScheduled -= h
            )
            .Select(ep => ep.EventArgs);
    }
    
    public static IObservable<SimulationCommandEventArgs> CommandStartingObservable
    (
        this ISimulationManager simulationManager)
    {
        return Observable
            .FromEventPattern<EventHandler<SimulationCommandEventArgs>, SimulationCommandEventArgs>
            (
                h => simulationManager.CommandStarting += h,
                h => simulationManager.CommandStarting -= h
            )
            .Select(ep => ep.EventArgs);
    }
    
    public static IObservable<SimulationCommandCompletedEventArgs> CommandCompletedObservable
    (
        this ISimulationManager simulationManager)
    {
        return Observable
            .FromEventPattern<EventHandler<SimulationCommandCompletedEventArgs>, SimulationCommandCompletedEventArgs>
            (
                h => simulationManager.CommandCompleted += h,
                h => simulationManager.CommandCompleted -= h
            )
            .Select(ep => ep.EventArgs);
    }
}