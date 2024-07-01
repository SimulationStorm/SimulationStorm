using System;

namespace SimulationStorm.AppSaves.Persistence;

public class AppSavesEntityFactory : IAppSavesEntityFactory
{
    public AppSave CreateAppSave(string name) => new()
    {
        Name = name
    };
    
    public ServiceSave CreateServiceSave(Type saveObjectType, object saveObject) => new()
    {
        SaveObjectType = saveObjectType,
        SaveObject = saveObject
    };
}