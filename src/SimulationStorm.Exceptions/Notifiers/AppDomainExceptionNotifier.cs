using System;
using SimulationStorm.Exceptions.FirstChance;
using SimulationStorm.Exceptions.Unhandled;
using UnhandledExceptionEventArgs = SimulationStorm.Exceptions.Unhandled.UnhandledExceptionEventArgs;

namespace SimulationStorm.Exceptions.Notifiers;

public class AppDomainExceptionNotifier : INotifyFirstChanceException, INotifyUnhandledException, IDisposable
{
    #region Events
    public event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException;
    
    public event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;
    #endregion

    private readonly AppDomain _appDomain;

    public AppDomainExceptionNotifier(AppDomain appDomain)
    {
        _appDomain = appDomain;
        _appDomain.FirstChanceException += OnFirstChanceException;
        _appDomain.UnhandledException += OnUnhandledException;
    }

    #region Event handlers
    private void OnFirstChanceException(object? _, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
        FirstChanceException?.Invoke(this, new FirstChanceExceptionEventArgs(e.Exception));
    }

    private void OnUnhandledException(object _, System.UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
            UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(exception, false));
    }
    #endregion

    public void Dispose()
    {
        _appDomain.FirstChanceException -= OnFirstChanceException;
        _appDomain.UnhandledException -= OnUnhandledException;
        GC.SuppressFinalize(this);
    }
}