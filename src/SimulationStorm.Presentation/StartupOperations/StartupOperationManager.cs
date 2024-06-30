using System.Collections.Generic;
using DotNext.Collections.Generic;

namespace SimulationStorm.Presentation.StartupOperations;

public class StartupOperationManager(IEnumerable<IStartupOperation> startupOperations) : IStartupOperationManager
{
    public void ExecuteStartupOperations() =>
        startupOperations.ForEach(startupOperation => startupOperation.OnStartup());
}