using SimulationStorm.Primitives;

namespace SimulationStorm.AppSaves.Persistence;

public class AppSavesOptions
{
    public string DatabaseDirectoryPath { get; init; } = null!;

    public string DatabaseFileName { get; init; } = null!;
    
    public Range<int> AppSaveNameLengthRange { get; init; }
}