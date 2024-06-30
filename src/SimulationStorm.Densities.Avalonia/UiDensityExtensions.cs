using System.ComponentModel;
using SimulationStorm.Densities.Presentation;
using ActiproUiDensity = ActiproSoftware.UI.Avalonia.Themes.UserInterfaceDensity;

namespace SimulationStorm.Densities.Avalonia;

public static class UiDensityExtensions
{
    public static ActiproUiDensity ToActipro(this UiDensity uiDensity) => uiDensity switch
    {
        UiDensity.Compact => ActiproUiDensity.Compact,
        UiDensity.Normal => ActiproUiDensity.Normal,
        UiDensity.Spacious => ActiproUiDensity.Spacious,
        _ => throw new InvalidEnumArgumentException(nameof(uiDensity), (int)uiDensity, typeof(UiDensity))
    };
    
    public static UiDensity ToUiDensity(this ActiproUiDensity actiproUiDensity) => actiproUiDensity switch
    {
        ActiproUiDensity.Compact => UiDensity.Compact,
        ActiproUiDensity.Normal => UiDensity.Normal,
        ActiproUiDensity.Spacious => UiDensity.Spacious,
        _ => throw new InvalidEnumArgumentException(nameof(actiproUiDensity), (int)actiproUiDensity, typeof(ActiproUiDensity))
    };
}