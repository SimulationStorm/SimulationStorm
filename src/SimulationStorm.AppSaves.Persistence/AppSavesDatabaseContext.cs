using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SimulationStorm.AppSaves.Entities;
using SimulationStorm.AppSaves.Persistence.EntityConfigurations;

namespace SimulationStorm.AppSaves.Persistence;

public sealed class AppSavesDatabaseContext : DbContext
{
    #region Tables
    public DbSet<AppSave> AppSaves => Set<AppSave>();

    public DbSet<ServiceSave> ServiceSaves => Set<ServiceSave>();
    #endregion

    private readonly AppSavesOptions _options;
    
    public AppSavesDatabaseContext(AppSavesOptions options)
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
        modelBuilder.ApplyConfiguration(new AppSaveConfiguration(_options));
        modelBuilder.ApplyConfiguration(new AppServiceSaveConfiguration());
    }
}