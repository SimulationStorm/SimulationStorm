using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Kernel;

namespace SimulationStorm.Collections.Extensions;

public static class ObservableListExtensions
{
    public static IObservable<IChangeSet<T>> OnItemsRemoved<T>
    (
        this IObservable<IChangeSet<T>> source,
        Action<IEnumerable<T>> removeAction,
        bool invokeOnUnsubscribe = true
    )
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(removeAction, nameof(removeAction));

        return new OnItemsBeingRemoved<T>(source, removeAction, invokeOnUnsubscribe).Run();
    }

    private sealed class OnItemsBeingRemoved<T>
    (
        IObservable<IChangeSet<T>> source,
        Action<IEnumerable<T>> callback,
        bool invokeOnUnsubscribe
    )
        where T : notnull
    {
        #region Fields
        private readonly Action<IEnumerable<T>> _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        
        private readonly IObservable<IChangeSet<T>> _source = source ?? throw new ArgumentNullException(nameof(source));
        #endregion

        public IObservable<IChangeSet<T>> Run() => Observable.Create<IChangeSet<T>>(observer =>
        {
            var items = new List<T>();
            var locker = new object();
            
            var subscriber = _source
                .Synchronize(locker)
                .Do(changes => RegisterForRemoval(items, changes), observer.OnError)
                .SubscribeSafe(observer);

            return Disposable.Create(items, passedItems =>
            {
                subscriber.Dispose();

                if (invokeOnUnsubscribe)
                    _callback(passedItems);
            });
        });

        private void RegisterForRemoval(IList<T> items, IChangeSet<T> changes)
        {
            foreach (var change in changes)
            {
                switch (change.Reason)
                {
                    case ListChangeReason.Replace:
                        change.Item.Previous.IfHasValue(item => _callback(Enumerable.Repeat(item, 1)));
                        break;

                    case ListChangeReason.Remove:
                        _callback(Enumerable.Repeat(change.Item.Current, 1));
                        break;

                    case ListChangeReason.RemoveRange:
                        _callback(change.Range);
                        break;

                    case ListChangeReason.Clear:
                        _callback(items);
                        break;
                }
            }

            items.Clone(changes);
        }
    }
}