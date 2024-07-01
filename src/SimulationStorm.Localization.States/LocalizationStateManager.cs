using System.Globalization;
using SimulationStorm.AppSaves;
using SimulationStorm.Localization.Presentation;

namespace SimulationStorm.Localization.States;

public class LocalizationSaveManager(ILocalizationManager localizationManager) : ServiceSaveManagerBase<CultureInfo>
{
    protected override CultureInfo SaveServiceCore() => localizationManager.CurrentCulture;

    protected override void RestoreServiceSaveCore(CultureInfo save) => localizationManager.ChangeCulture(save);
}