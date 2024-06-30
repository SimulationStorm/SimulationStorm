using System.Reactive.Concurrency;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.GameOfLife.Presentation.Management;
using SimulationStorm.Primitives;
using SimulationStorm.Threading.Presentation;

namespace SimulationStorm.GameOfLife.Presentation.Population;

public partial class PopulationViewModel
(
    IUiThreadScheduler uiThreadScheduler,
    GameOfLifeManager gameOfLifeManager,
    PopulationOptions options
)
    : ObservableObject
{
    #region Properties
    private double _cellBirthProbability = options.CellBirthProbability;
    public double CellBirthProbability
    {
        get => _cellBirthProbability;
        set => uiThreadScheduler.Schedule(() => SetProperty(ref _cellBirthProbability, value));
    }
    
    public Range<double> CellBirthProbabilityRange => options.CellBirthProbabilityRange;
    #endregion

    [RelayCommand]
    private Task PopulateAsync() => gameOfLifeManager.PopulateRandomlyAsync(CellBirthProbability);
}