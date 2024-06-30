using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Flyouts.Avalonia;

public partial class FlyoutHelper : AvaloniaObject
{
    static FlyoutHelper()
    {
        IsAppearanceAnimatedProperty.Changed.AddClassHandler<FlyoutPresenter>(HandleIsAppearanceAnimated);
        AppearanceAnimationDurationProperty.Changed.AddClassHandler<FlyoutPresenter>(HandleAppearanceAnimationDurationChanged);
    }
}