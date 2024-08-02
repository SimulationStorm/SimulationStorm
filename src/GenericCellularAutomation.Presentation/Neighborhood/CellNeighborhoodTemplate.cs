using System.Collections.Generic;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Presentation.Neighborhood;

public sealed class CellNeighborhoodTemplate(string name, NeighborhoodPositionSelector positionSelector)
{
    public string Name { get; } = name;

    public CellNeighborhood BuildNeighborhood(int radius)
    {
        var positions = new HashSet<Point>();
        
        CellNeighborhood.ForEachPositionWithinRadius(radius, position =>
        {
            if (positionSelector(radius, position.X, position.Y))
                positions.Add(position);
        });

        return new CellNeighborhood(radius, positions);
    }
}