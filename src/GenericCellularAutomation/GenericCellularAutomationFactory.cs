namespace GenericCellularAutomation;

public sealed class GenericCellularAutomationFactory : IGenericCellularAutomationFactory
{
    public IGenericCellularAutomation CreateGenericCellularAutomation() =>
        new GenericCellularAutomation();
}