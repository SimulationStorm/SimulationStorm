using System.Threading.Tasks;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats.DataTypes;
using SimulationStorm.Utilities;

namespace SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;

public class SummaryStatsManager<TSummary>
(
    IUniversalCollectionFactory universalCollectionFactory,
    IIntervalActionExecutor intervalActionExecutor,
    ISummarizableSimulationManager<TSummary> simulationManager,
    ISummaryStatsOptions options
)
    : CollectionManagerBase<SummaryRecord<TSummary>>(universalCollectionFactory, intervalActionExecutor, options),
        ISummaryStatsManager<TSummary>
{
    public async Task HandleSimulationCommandCompletedAsync(SimulationCommandCompletedEventArgs e)
    {
        if (!IsSummarizingNeeded(e.Command))
            return;
        
        await SummarizeSimulationAndAddToCollection(e.Command)
            .ConfigureAwait(false);
    }
    
    private bool IsSummarizingNeeded(SimulationCommand command) =>
        !IsDisposingOrDisposed
        && command.ChangesWorld
        && command is not RestoreStateCommand { IsRestoringFromAppSave: true }
        && IntervalActionExecutor.GetIsExecutionNeededAndMoveNext();
    
    private async Task SummarizeSimulationAndAddToCollection(SimulationCommand command)
    {
        var benchmarkResult = await simulationManager
            .SummarizeAndMeasureAsync()
            .ConfigureAwait(false);
        
        var summary = benchmarkResult.FunctionResult!;
        var elapsedTime = benchmarkResult.ElapsedTime;
        
        var records = new SummaryRecord<TSummary>(command, summary, elapsedTime);
        Collection.Add(records);
    }
}