using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimulationStorm.AppStates.Persistence;

public class AppStateRepository(AppStatesDatabaseContext dbContext) : IAppStateRepository
{
    public async Task AddAsync(AppState appState)
    {
        appState.DateAndTime = DateTime.Now;
        
        await dbContext.AppStates
            .AddAsync(appState)
            .ConfigureAwait(false);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public IEnumerable<AppState> GetAll() => dbContext.AppStates;

    public async Task UpdateAsync(AppState appState)
    {
        dbContext.AppStates.Update(appState);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public async Task DeleteAsync(AppState appState)
    {
        dbContext.AppStates.Remove(appState);
        
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public Task DeleteAllAsync() => dbContext.AppStates.ExecuteDeleteAsync();
}