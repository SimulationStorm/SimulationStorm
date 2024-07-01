using SimulationStorm.AppSaves;

namespace SimulationStorm.Densities.Presentation;

public class UiDensitySaveManager(IUiDensityManager uiDensityManager) : ServiceSaveManagerBase<UiDensity>
{
    protected override UiDensity SaveServiceCore() => uiDensityManager.CurrentDensity;

    protected override void RestoreServiceSaveCore(UiDensity save) => uiDensityManager.ChangeDensity(save);
}