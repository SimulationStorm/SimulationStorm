using SimulationStorm.Collections.Presentation;
using SimulationStorm.Simulation.History.Presentation.Models;

namespace SimulationStorm.Simulation.History.Presentation.Services;

public class HistoryState<TState> : CollectionAndManagerStateBase<HistoryRecord<TState>>;