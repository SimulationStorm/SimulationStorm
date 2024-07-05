using SimulationStorm.AppSaves;

namespace SimulationStorm.GameOfLife.Presentation.Population;

public class PopulationViewModelSaveManager(PopulationViewModel populationViewModel) :
    ServiceSaveManagerBase<PopulationViewModelSave>
{
    protected override PopulationViewModelSave SaveServiceCore() => new()
    {
        CellBirthProbability = populationViewModel.CellBirthProbability
    };

    protected override void RestoreServiceSaveCore(PopulationViewModelSave save) =>
        populationViewModel.CellBirthProbability = save.CellBirthProbability;
}