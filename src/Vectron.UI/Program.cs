using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Vectron.Core.ServerDiscovery;
using Vectron.UI.Platform.Avalonia;
using Vectron.UI.ViewModels;
using Vectron.UI.Views;

namespace Vectron.UI;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    // [STAThread]
    // public static void Main(string[] args) => BuildAvaloniaApp()
    //     .StartWithClassicDesktopLifetime(args);

    public static async Task Main(string[] args)
    {
        var hostBuilder = Host
            .CreateDefaultBuilder(args)
            .ConfigureAvaloniaAppBuilder<App>(appBuilder => appBuilder
                .UsePlatformDetect()
                .WithInterFont().LogToTrace().UseReactiveUI().SetupWithClassicDesktopLifetime(args))
            .ConfigureServices((host, services) =>
            {
                services.AddServerDiscovery("https://localhost");
                services
                    .AddSingleton<ViewLocator>()
                    .AddSingleton<MainWindowViewModel>()
                    .AddSingleton<LoginViewModel>()
                    .AddSingleton<IScreen, MainWindowViewModel>(sp => sp.GetRequiredService<MainWindowViewModel>());
            });
        
        var host = hostBuilder.Build();

        if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            host.RunAvaloniaApp();
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            await host.RunAvaloniaAppAsync();
        else
            throw new NotSupportedException($"Unsupported platform: {RuntimeInformation.OSDescription}");
    }
    
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure(() => new App(BuildServices()))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }

    public static IServiceProvider BuildServices()
    {
        return Host
            .CreateDefaultBuilder([])
            .ConfigureAvaloniaAppBuilder<App>(appBuilder => appBuilder
                .UsePlatformDetect()
                .WithInterFont().LogToTrace().UseReactiveUI().SetupWithClassicDesktopLifetime([]))
            .ConfigureServices((host, services) =>
            {
                services.AddServerDiscovery("https://localhost");
                services
                    .AddSingleton<ViewLocator>()
                    .AddSingleton<MainWindowViewModel>();
            }).Build().Services;
    }
}