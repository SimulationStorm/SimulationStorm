using System;
using Avalonia.Threading;
using SimulationStorm.Exceptions.FirstChance;
using SimulationStorm.Exceptions.Unhandled;
using UnhandledExceptionEventArgs = SimulationStorm.Exceptions.Unhandled.UnhandledExceptionEventArgs;

namespace SimulationStorm.Exceptions.Avalonia;

public class DispatcherExceptionNotifier : INotifyFirstChanceException, INotifyUnhandledException, IDisposable
{
    #region Events
    public event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException;
    
    public event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;
    #endregion

    private readonly Dispatcher _dispatcher;
    
    public DispatcherExceptionNotifier(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _dispatcher.UnhandledExceptionFilter += OnUnhandledExceptionFilter;
        _dispatcher.UnhandledException += OnUnhandledException;
    }

    #region Event handlers
    private void OnUnhandledExceptionFilter(object _, DispatcherUnhandledExceptionFilterEventArgs e)
    {
        FirstChanceException?.Invoke(this, new FirstChanceExceptionEventArgs(e.Exception));
    }

    private void OnUnhandledException(object _, DispatcherUnhandledExceptionEventArgs e)
    {
        var eventArgs = new UnhandledExceptionEventArgs(e.Exception, true);
        UnhandledException?.Invoke(this, eventArgs);

        e.Handled = eventArgs.Handled;
    }
    #endregion

    public void Dispose()
    {
        _dispatcher.UnhandledExceptionFilter -= OnUnhandledExceptionFilter;
        _dispatcher.UnhandledException -= OnUnhandledException;
        GC.SuppressFinalize(this);
    }
}