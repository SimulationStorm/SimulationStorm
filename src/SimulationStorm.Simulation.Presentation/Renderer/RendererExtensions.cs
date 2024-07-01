using System;
using System.Reactive.Linq;
using SimulationStorm.Utilities.Progress;

namespace SimulationStorm.Simulation.Presentation.Renderer;

public static class RendererExtensions
{
    public static IObservable<EventArgs> RenderingStartingObservable
    (
        this IRenderer renderer)
    {
        return Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => renderer.RenderingStarting += h,
                h => renderer.RenderingStarting -= h
            )
            .Select(ep => ep.EventArgs);
    }
    
    public static IObservable<RenderingCompletedEventArgs> RenderingCompletedObservable
    (
        this IRenderer renderer)
    {
        return Observable
            .FromEventPattern<EventHandler<RenderingCompletedEventArgs>, RenderingCompletedEventArgs>
            (
                h => renderer.RenderingCompleted += h,
                h => renderer.RenderingCompleted -= h
            )
            .Select(ep => ep.EventArgs);
    }
    
    public static IObservable<CancellableProgressChangedEventArgs> RenderingProgressChangedObservable
    (
        this IProgressedRenderer renderer)
    {
        return Observable
            .FromEventPattern<EventHandler<CancellableProgressChangedEventArgs>, CancellableProgressChangedEventArgs>
            (
                h => renderer.ProgressChanged += h,
                h => renderer.ProgressChanged -= h
            )
            .Select(ep => ep.EventArgs);
    }
}