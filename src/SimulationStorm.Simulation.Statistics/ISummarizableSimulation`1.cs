namespace SimulationStorm.Simulation.Statistics;

public interface ISummarizableSimulation<out TSummary> : ISimulation
{
    TSummary Summarize();
}