using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using SimulationStorm.Flyouts.Presentation;

namespace SimulationStorm.Flyouts.Avalonia;

public static class FlyoutUtils
{
    public static IDisposable CreateViewModeledFlyout(IFlyoutViewModel flyoutViewModel, out Flyout flyout)
    {
        var localFlyout = new Flyout
        {
            Content = flyoutViewModel
        };
        
        var disposables = new CompositeDisposable(2);

        var closeRequestedFromViewModel = false;
        
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => flyoutViewModel.CloseRequested += h,
                h => flyoutViewModel.CloseRequested -= h
            )
            .Subscribe(_ =>
            {
                closeRequestedFromViewModel = true;
                localFlyout.Hide();
                closeRequestedFromViewModel = false;
            })
            .DisposeWith(disposables);

        Observable
            .FromEventPattern<EventHandler<CancelEventArgs>, CancelEventArgs>
            (
                h => localFlyout.Closing += h,
                h => localFlyout.Closing -= h
            )
            .Select(e => e.EventArgs)
            .Subscribe(e =>
            {
                if (!closeRequestedFromViewModel)
                    e.Cancel = !flyoutViewModel.IsBackgroundClosingAllowed;
                
                if (e.Cancel is false)
                    flyoutViewModel.OnClosing();
            })
            .DisposeWith(disposables);

        flyout = localFlyout;
        return disposables;
    }
}