namespace SimulationStorm.Simulation.History;

public interface ISaveableSimulation<TSave> : ISimulation
{
    TSave Save();
    
    void RestoreState(TSave save);
}