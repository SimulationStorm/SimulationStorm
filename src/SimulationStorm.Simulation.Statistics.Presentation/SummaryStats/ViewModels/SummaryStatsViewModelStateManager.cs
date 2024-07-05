namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.ViewModels;

public class SummaryStatsViewModelSaveManager
(
    ISummaryStatsViewModel summaryStatsViewModel
)
    : StatsViewModelSaveManagerBase<SummaryStatsViewModelSave>(summaryStatsViewModel);