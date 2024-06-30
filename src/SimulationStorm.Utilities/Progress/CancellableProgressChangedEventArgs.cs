using System.ComponentModel;

namespace SimulationStorm.Utilities.Progress;

public class CancellableProgressChangedEventArgs(int progressPercentage) : CancelEventArgs
{
    public int ProgressPercentage { get; } = progressPercentage;
}