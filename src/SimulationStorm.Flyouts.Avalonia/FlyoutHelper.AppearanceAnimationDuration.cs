using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;

namespace SimulationStorm.Flyouts.Avalonia;

public partial class FlyoutHelper
{
    public static readonly AttachedProperty<TimeSpan> AppearanceAnimationDurationProperty =
        AvaloniaProperty.RegisterAttached<FlyoutHelper, FlyoutPresenter, TimeSpan>("AppearanceAnimationDuration");

    public static TimeSpan GetAppearanceAnimationDuration(FlyoutPresenter flyoutPresenter) =>
        flyoutPresenter.GetValue(AppearanceAnimationDurationProperty);
    
    public static void SetAppearanceAnimationDuration(FlyoutPresenter flyoutPresenter, TimeSpan animationDuration) =>
        flyoutPresenter.SetValue(AppearanceAnimationDurationProperty, animationDuration);

    private static void HandleAppearanceAnimationDurationChanged(FlyoutPresenter flyoutPresenter, AvaloniaPropertyChangedEventArgs e)
    {
        var animationDuration = e.GetNewValue<TimeSpan>();

        flyoutPresenter.Transitions ??= [];
        
        var opacityTransition = flyoutPresenter.Transitions
            .OfType<DoubleTransition>()
            .FirstOrDefault(transition => transition.Property == Visual.OpacityProperty);

        if (opacityTransition is null)
        {
            opacityTransition = new DoubleTransition
            {
                Property = Visual.OpacityProperty
            };
            flyoutPresenter.Transitions.Add(opacityTransition);
        }

        opacityTransition.Duration = animationDuration;
    }
}