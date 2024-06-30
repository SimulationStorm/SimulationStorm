using SimulationStorm.AppStates;

namespace SimulationStorm.Densities.Presentation;

public class UiDensityStateManager(IUiDensityManager uiDensityManager) : ServiceStateManagerBase<UiDensity>
{
    protected override UiDensity SaveServiceStateImpl() => uiDensityManager.CurrentDensity;

    protected override void RestoreServiceStateImpl(UiDensity state) => uiDensityManager.ChangeDensity(state);
}