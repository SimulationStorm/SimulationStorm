using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.History.Presentation.Models;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistorySave<TState> : CollectionAndManagerSaveBase<HistoryRecord<TState>>;