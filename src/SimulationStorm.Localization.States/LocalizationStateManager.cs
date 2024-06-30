using System.Globalization;
using SimulationStorm.AppStates;
using SimulationStorm.Localization.Presentation;

namespace SimulationStorm.Localization.States;

public class LocalizationStateManager(ILocalizationManager localizationManager) : ServiceStateManagerBase<CultureInfo>
{
    protected override CultureInfo SaveServiceStateImpl() => localizationManager.CurrentCulture;

    protected override void RestoreServiceStateImpl(CultureInfo state) => localizationManager.ChangeCulture(state);
}