namespace SimulationStorm.Logging;

public class LoggingOptions
{
    public string LogTemplate { get; init; } = null!;

    public string LogFilesDirectoryPath { get; init; } = null!;

    public string LogsFileName { get; init; } = null!;
}