using System;
using Avalonia.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Vectron.UI.Platform.Avalonia;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AttachLoggerToAvalonia(this IServiceCollection services, params string[] areas) =>
        services.AddSingleton<ILogSink>(sp => new LoggerSink(sp.GetRequiredService<ILogger<LoggerSink>>(), areas));
}