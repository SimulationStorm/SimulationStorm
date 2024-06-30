namespace SimulationStorm.Simulation.Resetting;

public interface IResettableSimulation : ISimulation
{
    /// <summary>
    /// Returns the simulation to its initial state.
    /// </summary>
    void Reset();
}