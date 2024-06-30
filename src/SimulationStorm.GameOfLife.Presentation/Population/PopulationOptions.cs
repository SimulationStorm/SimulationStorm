using SimulationStorm.Primitives;

namespace SimulationStorm.GameOfLife.Presentation.Population;

public class PopulationOptions
{
    public Range<double> CellBirthProbabilityRange { get; init; }
    
    public double CellBirthProbability { get; init; }
}