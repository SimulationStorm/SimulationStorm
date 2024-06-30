using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Bounded;

public interface IBoundedSimulation : ISimulation
{
    Size WorldSize { get; }

    bool IsValidWorldSize(Size size);
    
    void ChangeWorldSize(Size newSize);
}