using System;

namespace SimulationStorm.AppSaves;

public interface IAppSavesEntityFactory
{
    AppSave CreateAppSave(string name);
    
    ServiceSave CreateServiceSave(Type saveObjectType, object saveObject);
}