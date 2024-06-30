using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace SimulationStorm.Avalonia.Behaviors;

public class CloseFlyoutOnClickBehavior : Behavior<Button>
{
    private IDisposable? _clickEventSubscription;

    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();

        var flyoutPresenter = AssociatedObject!.FindAncestorOfType<FlyoutPresenter>();
        if (flyoutPresenter?.Parent is not Popup popup)
            return;

        _clickEventSubscription = Observable
            .FromEventPattern<RoutedEventArgs>
            (
                h => AssociatedObject!.Click += h,
                h => AssociatedObject!.Click -= h
            )
            .Subscribe(_ =>
            {
                if (AssociatedObject!.Command is not null && AssociatedObject.IsEnabled)
                    AssociatedObject.Command.Execute(AssociatedObject.CommandParameter);

                popup.Close();
            });
    }

    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        _clickEventSubscription?.Dispose();
    }
}