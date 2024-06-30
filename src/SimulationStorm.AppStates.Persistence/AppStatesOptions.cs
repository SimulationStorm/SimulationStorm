using SimulationStorm.Primitives;

namespace SimulationStorm.AppStates.Persistence;

public class AppStatesOptions
{
    public string DatabaseDirectoryPath { get; init; } = null!;

    public string DatabaseFileName { get; init; } = null!;
    
    public Range<int> AppStateNameLengthRange { get; init; }
}