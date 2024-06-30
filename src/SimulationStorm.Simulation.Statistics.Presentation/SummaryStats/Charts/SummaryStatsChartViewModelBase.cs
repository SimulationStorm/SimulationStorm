using SimulationStorm.Collections.Presentation;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Simulation.Statistics.Presentation.Charts;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.Charts;

public abstract class SummaryStatsChartViewModelBase<TSummary, TRecordModel>
(
    IUiThreadScheduler uiThreadScheduler,
    ICollectionManager<SummaryRecord<TSummary>> collectionManager,
    ILocalizationManager localizationManager
)
    : ChartViewModelBase<SummaryRecord<TSummary>, TRecordModel>(uiThreadScheduler, collectionManager, localizationManager)
    where TRecordModel : class, IIndexedObject
{
    protected abstract override TRecordModel CreateRecordModel(SummaryRecord<TSummary> record);
}