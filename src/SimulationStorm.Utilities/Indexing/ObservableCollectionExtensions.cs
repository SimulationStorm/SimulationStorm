using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DotNext.Collections.Generic;
using DynamicData;
using DynamicData.Binding;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Utilities.Indexing;

public static class ObservableCollectionExtensions
{
    public static IDisposable IndexItemsAndBind<TCollection, TItem, TIndexedItem>
    (
        this TCollection observableCollection,
        Func<TItem, TIndexedItem> indexedItemFactory,
        out ReadOnlyObservableCollection<TIndexedItem> readOnlyObservableCollection,
        IScheduler? scheduler = null
    )
        where TCollection : INotifyCollectionChanged, IEnumerable<TItem>
        where TItem : notnull
        where TIndexedItem : IIndexedObject
    {
        var itemStream = observableCollection.ToObservableChangeSet<TCollection, TItem>();
        
        if (scheduler is not null)
            itemStream = itemStream.ObserveOn(scheduler);

        var indexedItemStream = itemStream
            .Transform(indexedItemFactory)
            .Bind(out var indexedItems);
        
        readOnlyObservableCollection = indexedItems;

        var subscriptions = new CompositeDisposable(2);

        var indexedItemsSubscription = indexedItems
            .ObserveCollectionChanges()
            .Select(e => e.EventArgs)
            .Subscribe(e =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:
                    {
                        var index = 0;
                        indexedItems.ForEach(indexedItem => indexedItem.Index = index++);
                        break;
                    }
                }
            });
        
        subscriptions.AddRange(indexedItemsSubscription, indexedItemStream.Subscribe());
        return subscriptions;
    }
}