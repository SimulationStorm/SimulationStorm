using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimulationStorm.AppSaves.Entities;

namespace SimulationStorm.AppSaves.Persistence;

public class AppSaveRepository(AppSavesDatabaseContext dbContext) : IAppSaveRepository
{
    public async Task AddAppSaveAsync(AppSave appSave)
    {
        appSave.DateAndTime = DateTime.Now;
        
        await dbContext.AppSaves
            .AddAsync(appSave)
            .ConfigureAwait(false);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public IEnumerable<AppSave> GetAllAppSaves() => dbContext.AppSaves;

    public async Task UpdateAppSaveAsync(AppSave appSave)
    {
        dbContext.AppSaves.Update(appSave);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public async Task DeleteAppSaveAsync(AppSave appSave)
    {
        dbContext.AppSaves.Remove(appSave);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public Task DeleteAllAppSavesAsync() => dbContext.AppSaves.ExecuteDeleteAsync();
}