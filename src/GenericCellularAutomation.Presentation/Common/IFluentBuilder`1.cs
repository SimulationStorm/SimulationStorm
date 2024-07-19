namespace GenericCellularAutomation.Presentation.Common;

public interface IFluentBuilder<out T>
{
    T Build();
}