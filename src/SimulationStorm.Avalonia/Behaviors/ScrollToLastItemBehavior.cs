using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using DynamicData.Binding;

namespace SimulationStorm.Avalonia.Behaviors;

public class ScrollToLastItemBehavior : Behavior<DataGrid>
{
    #region Fields
    private IDisposable _itemsSourceSubscription = null!;

    private IDisposable? _itemCollectionSubscription;
    #endregion

    #region Protected methods
    protected override void OnAttached()
    {
        base.OnAttached();
        _itemsSourceSubscription = SubscribeOnItemsSourcePropertyChange();
    }

    protected override void OnDetaching()
    {
        _itemsSourceSubscription.Dispose();
        _itemCollectionSubscription?.Dispose();
    }
    #endregion

    #region Private methods
    private IDisposable SubscribeOnItemsSourcePropertyChange() => AssociatedObject!
        .GetObservable(DataGrid.ItemsSourceProperty)
        .Subscribe(OnItemsSourceChanged);

    private void OnItemsSourceChanged(IEnumerable itemsSource)
    {
        _itemCollectionSubscription?.Dispose();
        CreateItemCollectionSubscriptionIfItIsObservable(itemsSource);
    }

    private void CreateItemCollectionSubscriptionIfItIsObservable(IEnumerable itemsSource)
    {
        if (itemsSource is INotifyCollectionChanged itemCollection)
            _itemCollectionSubscription = SubscribeOnItemCollectionChange(itemCollection);
    }
    
    private IDisposable SubscribeOnItemCollectionChange(INotifyCollectionChanged itemCollection) => itemCollection
        .ObserveCollectionChanges()
        .Select(e => e.EventArgs)
        .Subscribe(OnItemCollectionChanged);
    
    private void OnItemCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is not NotifyCollectionChangedAction.Add
            && e.Action is not NotifyCollectionChangedAction.Remove
            && e.Action is not NotifyCollectionChangedAction.Reset)
            return;

        var items = AssociatedObject!.ItemsSource.Cast<object>();
        var lastItem = items.LastOrDefault();
        if (lastItem is not null)
            AssociatedObject!.ScrollIntoView(lastItem, null);
    }
    #endregion
}