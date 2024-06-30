using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using SimulationStorm.Avalonia.Extensions;

namespace SimulationStorm.Flyouts.Avalonia;

public partial class FlyoutHelper
{
    public static readonly AttachedProperty<bool> IsAppearanceAnimatedProperty =
        AvaloniaProperty.RegisterAttached<FlyoutHelper, FlyoutPresenter, bool>("IsAppearanceAnimated");
    
    public static bool GetIsAppearanceAnimated(FlyoutPresenter flyoutPresenter) =>
        flyoutPresenter.GetValue(IsAppearanceAnimatedProperty);
    
    public static void SetIsAppearanceAnimated(FlyoutPresenter flyoutPresenter, bool isAppearanceAnimated) =>
        flyoutPresenter.SetValue(IsAppearanceAnimatedProperty, isAppearanceAnimated);

    private static void HandleIsAppearanceAnimated(FlyoutPresenter flyoutPresenter, AvaloniaPropertyChangedEventArgs e)
    {
        var isAppearanceAnimated = e.GetNewValue<bool>();
        if (isAppearanceAnimated)
            flyoutPresenter.AttachedToLogicalTree += HandleFlyoutPresenterAttachedToLogicalTree;
        else
            flyoutPresenter.AttachedToLogicalTree -= HandleFlyoutPresenterAttachedToLogicalTree;
    }

    private static void HandleFlyoutPresenterAttachedToLogicalTree(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        var flyoutPresenter = (FlyoutPresenter)sender!;
        var popup = flyoutPresenter.FindLogicalAncestorOfType<Popup>()!;
        
        flyoutPresenter.Opacity = 0;
        
        popup.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => popup.Opened += h,
                    h => popup.Opened -= h
                )
                .Subscribe(_ => flyoutPresenter.Opacity = 1)
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<EventArgs>, EventArgs>
                (
                    h => popup.Closed += h,
                    h => popup.Closed -= h
                )
                .Subscribe(_ => flyoutPresenter.Opacity = 0)
                .DisposeWith(disposables);
        });
    }
}