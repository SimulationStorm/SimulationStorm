using System;
using System.Reactive.Linq;

namespace SimulationStorm.Simulation.Presentation.SimulationManager;

public static class SimulationManagerExtensions
{
    public static IObservable<SimulationCommandEventArgs> CommandSchedulingObservable
    (
        this ISimulationManager simulationManager)
    {
        return Observable
            .FromEventPattern<EventHandler<SimulationCommandEventArgs>, SimulationCommandEventArgs>
            (
                h => simulationManager.CommandScheduling += h,
                h => simulationManager.CommandScheduling -= h
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
    
    public static IObservable<SimulationCommandProgressChangedEventArgs> CommandProgressChangedObservable
    (
        this ISimulationManager commandScheduler)
    {
        return Observable
            .FromEventPattern<EventHandler<SimulationCommandProgressChangedEventArgs>, SimulationCommandProgressChangedEventArgs>
            (
                h => commandScheduler.CommandProgressChanged += h,
                h => commandScheduler.CommandProgressChanged -= h
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