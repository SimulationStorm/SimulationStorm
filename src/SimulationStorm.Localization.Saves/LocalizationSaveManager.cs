using System.Globalization;
using SimulationStorm.AppSaves;
using SimulationStorm.Localization.Presentation;

namespace SimulationStorm.Localization.Saves;

public class LocalizationSaveManager(ILocalizationManager localizationManager) : ServiceSaveManagerBase<CultureInfo>
{
    protected override CultureInfo SaveServiceCore() => localizationManager.CurrentCulture;

    protected override void RestoreServiceSaveCore(CultureInfo culture) => localizationManager.ChangeCulture(culture);
}