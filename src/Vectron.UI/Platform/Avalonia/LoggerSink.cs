using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Logging;
using Microsoft.Extensions.Logging;

namespace Vectron.UI.Platform.Avalonia;

public class LoggerSink : ILogSink
{
    private readonly ILogger<LoggerSink> _logger;
    private readonly IReadOnlyCollection<string> _selectedAreas;

    public LoggerSink(ILogger<LoggerSink> logger, params string[] areas)
    {
        _logger = logger;
        _selectedAreas = areas;
    }
    
    public bool IsEnabled(LogEventLevel level, string area) =>
        _logger.IsEnabled(FromLogEventLevel(level)) && _selectedAreas.Contains(area);

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        throw new System.NotImplementedException();
    }

    public void Log(LogEventLevel level, string area, object? source, string? messageTemplate, params object?[] propertyValues)
    {
        var concreteLevel = FromLogEventLevel(level);
        if(!_logger.IsEnabled(concreteLevel)) return;

        var eventId = $"AvaloniaHost[{area}]";
        if (source is not null) eventId = $"{eventId}+{Convert.ToString(source)}";
        
        _logger.Log(concreteLevel, new EventId(1, eventId), messageTemplate, propertyValues);
    }

    private static LogLevel FromLogEventLevel(LogEventLevel level) => level switch
    {
        LogEventLevel.Verbose => LogLevel.Trace,
        LogEventLevel.Debug => LogLevel.Debug,
        LogEventLevel.Information => LogLevel.Information,
        LogEventLevel.Warning => LogLevel.Warning,
        LogEventLevel.Error => LogLevel.Error,
        LogEventLevel.Fatal => LogLevel.Critical,
        _ => LogLevel.Trace
    };
}