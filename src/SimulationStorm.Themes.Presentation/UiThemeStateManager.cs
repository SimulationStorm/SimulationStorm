using SimulationStorm.AppStates;

namespace SimulationStorm.Themes.Presentation;

public class UiThemeStateManager(IUiThemeManager uiThemeManager) : ServiceStateManagerBase<UiTheme>
{
    protected override UiTheme SaveServiceStateImpl() => uiThemeManager.CurrentTheme;

    protected override void RestoreServiceStateImpl(UiTheme state) => uiThemeManager.ChangeTheme(state);
}