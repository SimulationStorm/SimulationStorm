using System;

namespace SimulationStorm.AppSaves;

public interface IServiceSaveManager
{
    Type SaveType { get; }
    
    object SaveService();

    void RestoreServiceSave(object save);
}