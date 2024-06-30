using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SimulationStorm.AppStates.Persistence.EntityConfigurations;

namespace SimulationStorm.AppStates.Persistence;

public sealed class AppStatesDatabaseContext : DbContext
{
    #region Tables
    public DbSet<AppState> AppStates => Set<AppState>();

    public DbSet<AppServiceState> AppServiceStates => Set<AppServiceState>();
    #endregion

    private readonly AppStatesOptions _options;
    
    public AppStatesDatabaseContext(AppStatesOptions options)
    {
        _options = options;
        
#if DEBUG
        Database.EnsureDeleted();
#endif
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ensure directory exists
        Directory.CreateDirectory(_options.DatabaseDirectoryPath);

        var databaseFilePath = Path.Combine(_options.DatabaseDirectoryPath, _options.DatabaseFileName);
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = databaseFilePath,
            Pooling = false // This is needed to ensure a connection will be closed after disposing db context.
        };
        var connectionString = connectionStringBuilder.ToString();
        
        optionsBuilder
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLazyLoadingProxies()
            .UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AppStateEntityConfiguration(_options));
        modelBuilder.ApplyConfiguration(new AppServiceStateEntityConfiguration());
    }
}