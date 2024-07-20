using GenericCellularAutomation.Presentation.Common;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateDescriptorBuilder : IFluentBuilder<CellStateDescriptor>
{
    #region Fields
    private byte _cellState;
    
    private string _name = string.Empty;

    private Color _color;
    #endregion

    #region Methods
    public CellStateDescriptorBuilder HasCellState(byte cellState)
    {
        _cellState = cellState;
        return this;
    }
    
    public CellStateDescriptorBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public CellStateDescriptorBuilder HasColor(Color color)
    {
        _color = color;
        return this;
    }

    public CellStateDescriptor Build() => new(_cellState, _name, _color);
    #endregion
}