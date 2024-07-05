using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DotNext.Collections.Generic;
using SimulationStorm.Exceptions.FirstChance;
using SimulationStorm.Utilities.Disposing;
using SimulationStorm.Exceptions.Unhandled;
using UnhandledExceptionEventArgs = SimulationStorm.Exceptions.Unhandled.UnhandledExceptionEventArgs;

namespace SimulationStorm.Exceptions;

public class ExceptionHandlersToNotifiersSubscriber : DisposableObject
{
    public ExceptionHandlersToNotifiersSubscriber
    (
        IEnumerable<INotifyFirstChanceException> firstChanceExceptionNotifiers,
        IEnumerable<IHandleFirstChanceException> firstChanceExceptionHandlers,
        IEnumerable<INotifyUnhandledException> unhandledExceptionNotifiers,
        IEnumerable<IHandleUnhandledException> unhandledExceptionHandlers)
    {
        SubscribeFirstChanceExceptionHandlers(
            firstChanceExceptionNotifiers, firstChanceExceptionHandlers, Disposables);
        
        SubscribeUnhandledExceptionHandlers(
            unhandledExceptionNotifiers, unhandledExceptionHandlers, Disposables);
    }

    #region First chance
    private static void SubscribeFirstChanceExceptionHandlers
    (
        IEnumerable<INotifyFirstChanceException> firstChanceExceptionNotifiers,
        IEnumerable<IHandleFirstChanceException> firstChanceExceptionHandlers,
        CompositeDisposable disposables)
    {
        firstChanceExceptionNotifiers.ForEach(firstChanceExceptionNotifier =>
        {
            firstChanceExceptionHandlers.ForEach(firstChanceExceptionHandler =>
            {
                SubscribeFirstChanceExceptionHandler(firstChanceExceptionNotifier, firstChanceExceptionHandler)
                    .DisposeWith(disposables);
            });
        });
    }

    private static IDisposable SubscribeFirstChanceExceptionHandler
    (
        INotifyFirstChanceException firstChanceExceptionNotifier,
        IHandleFirstChanceException firstChanceExceptionHandler)
    {
        return Observable
            .FromEventPattern<EventHandler<FirstChanceExceptionEventArgs>, FirstChanceExceptionEventArgs>
            (
                h => firstChanceExceptionNotifier.FirstChanceException += h,
                h => firstChanceExceptionNotifier.FirstChanceException -= h
            )
            .Subscribe(e => firstChanceExceptionHandler.HandleFirstChanceException(e.Sender!, e.EventArgs));
    }
    #endregion

    #region Unhanled
    private static void SubscribeUnhandledExceptionHandlers
    (
        IEnumerable<INotifyUnhandledException> unhandledExceptionNotifiers,
        IEnumerable<IHandleUnhandledException> unhandledExceptionHandlers,
        CompositeDisposable disposables)
    {
        unhandledExceptionNotifiers.ForEach(unhandledExceptionNotifier =>
        {
            unhandledExceptionHandlers.ForEach(unhandledExceptionHandler =>
            {
                SubscribeUnhandledExceptionHandler(unhandledExceptionNotifier, unhandledExceptionHandler)
                    .DisposeWith(disposables);
            });
        });
    }

    private static IDisposable SubscribeUnhandledExceptionHandler
    (
        INotifyUnhandledException unhandledExceptionNotifier,
        IHandleUnhandledException unhandledExceptionHandler)
    {
        return Observable
            .FromEventPattern<EventHandler<UnhandledExceptionEventArgs>, UnhandledExceptionEventArgs>
            (
                h => unhandledExceptionNotifier.UnhandledException += h,
                h => unhandledExceptionNotifier.UnhandledException -= h
            )
            .Subscribe(e => unhandledExceptionHandler.HandleUnhandledException(e.Sender!, e.EventArgs));
    }
    #endregion
}