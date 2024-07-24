using System.Collections.Generic;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateCollectionDescriptorBuilder : IFluentBuilder<CellStateCollectionDescriptor>
{
    #region Fields
    private readonly IList<CellStateDescriptor> _cellStates = new List<CellStateDescriptor>();

    private CellStateDescriptor _defaultCellState = null!;
    #endregion

    #region Methods
    public CellStateCollectionDescriptorBuilder HasCellState(CellStateDescriptor cellState)
    {
        _cellStates.Add(cellState);
        return this;
    }
    
    public CellStateCollectionDescriptorBuilder HasCellStates(params CellStateDescriptor[] cellStates)
    {
        _cellStates.AddAll(cellStates);
        return this;
    }

    public CellStateCollectionDescriptorBuilder HasDefaultCellState(CellStateDescriptor defaultCellState)
    {
        _defaultCellState = defaultCellState;
        return this;
    }

    public CellStateCollectionDescriptor Build() =>
        new((IReadOnlyList<CellStateDescriptor>)_cellStates, _defaultCellState);
    #endregion
}