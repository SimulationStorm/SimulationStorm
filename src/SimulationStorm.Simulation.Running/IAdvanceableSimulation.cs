namespace SimulationStorm.Simulation.Running;

public interface IAdvanceableSimulation : ISimulation
{
    /// <summary>
    /// Makes a simulation step.
    /// </summary>
    void Advance();
}