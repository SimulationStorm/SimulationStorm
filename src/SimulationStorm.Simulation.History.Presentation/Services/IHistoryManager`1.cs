using System;
using System.Threading.Tasks;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.Presentation.SimulationManager;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public interface IHistoryManager<TSave> : ICollectionManager<HistoryRecord<TSave>>
{
    event EventHandler<SimulationCommandCompletedEventArgs>? SimulationCommandExecutedEventHandled;
    
    bool CanGoToPreviousSave();
    Task GoToPreviousSaveAsync();

    bool CanGoToNextSave();
    Task GoToNextSaveAsync();

    bool CanGoToFirstSave();
    Task GoToFirstSaveAsync();

    bool CanGoToLastSave();
    Task GoToLastSaveAsync();

    bool CanGoToSave(int saveIndex);
    Task GoToSaveAsync(int saveIndex);

    Task DeleteSaveAsync(int saveIndex);
}