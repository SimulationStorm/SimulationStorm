using SimulationStorm.AppStates;

namespace SimulationStorm.GameOfLife.Presentation.Population;

public class PopulationViewModelStateManager(PopulationViewModel populationViewModel) :
    ServiceStateManagerBase<PopulationViewModelState>
{
    protected override PopulationViewModelState SaveServiceStateImpl() => new()
    {
        CellBirthProbability = populationViewModel.CellBirthProbability
    };

    protected override void RestoreServiceStateImpl(PopulationViewModelState state) =>
        populationViewModel.CellBirthProbability = state.CellBirthProbability;
}