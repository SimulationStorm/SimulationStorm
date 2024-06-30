using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace SimulationStorm.Avalonia.Behaviors;

public class ScrollToSelectedItemBehavior : Behavior<DataGrid>
{
    private IDisposable _selectedItemSubscription = null!;

    #region Protected methods
    protected override void OnAttached()
    {
        base.OnAttached();
        _selectedItemSubscription = CreateSelectedItemSubscription();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        _selectedItemSubscription.Dispose();
    }
    #endregion

    private IDisposable CreateSelectedItemSubscription() => AssociatedObject!
        .GetObservable(DataGrid.SelectedItemProperty)
        .Where(selectedItem => selectedItem is not null)
        .Subscribe(selectedItem => AssociatedObject!.ScrollIntoView(selectedItem, column: null));
}