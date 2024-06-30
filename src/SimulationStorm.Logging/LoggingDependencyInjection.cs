using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

namespace SimulationStorm.Logging;

public static class LoggingDependencyInjection
{
    // MS Extensions logging builder is not used because it registers unnecessary options services
    public static IServiceCollection AddLoggingServices(this IServiceCollection services, LoggingOptions options) => services
        .AddSingleton<ILoggerFactory, LoggerFactory>()
        .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
        .AddSingleton<ILoggerProvider>(_ => new SerilogLoggerProvider(CreateSerilogLogger(options), dispose: true));

    private static Logger CreateSerilogLogger(LoggingOptions options) => new LoggerConfiguration()
        .Enrich.WithThreadId()
        .WriteTo.File(GetLogsFilePath(options), outputTemplate: options.LogTemplate)
        .CreateLogger();

    private static string GetLogsFilePath(LoggingOptions options) =>
        Path.Combine(options.LogFilesDirectoryPath, options.LogsFileName);
}