using System;
using Avalonia;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Vectron.UI.Platform.Avalonia;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureAvaloniaAppBuilder<TApplication>(this IHostBuilder hostBuilder,
        Action<AppBuilder> configureAppbuilder, IHostedLifetime? lifetime = null)
        where TApplication : Application
    {
        ArgumentNullException.ThrowIfNull(configureAppbuilder);

        hostBuilder.ConfigureServices((ctx, s) =>
        {
            s.AddTransient<IDataTemplate, ViewLocator>();
            s.AddSingleton<TApplication>().AddSingleton(sp =>
            {
                var appBuilder = AppBuilder.Configure(sp.GetRequiredService<TApplication>);
                configureAppbuilder(appBuilder);
                return appBuilder;
            });
            s.AddSingleton<Application>(svc => svc.GetRequiredService<AppBuilder>().Instance!);
            s.AddSingleton<IHostedLifetime>(p => 
                lifetime ?? 
                HostedLifetime.Select(p.GetRequiredService<ILoggerFactory>(), p.GetRequiredService<Application>().ApplicationLifetime!));
        });
        
        return hostBuilder;
    }
}