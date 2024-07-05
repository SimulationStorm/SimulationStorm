using SimulationStorm.AppSaves;

namespace SimulationStorm.Themes.Presentation;

public class UiThemeSaveManager(IUiThemeManager uiThemeManager) : ServiceSaveManagerBase<UiTheme>
{
    protected override UiTheme SaveServiceCore() => uiThemeManager.CurrentTheme;

    protected override void RestoreServiceSaveCore(UiTheme save) => uiThemeManager.ChangeTheme(save);
}