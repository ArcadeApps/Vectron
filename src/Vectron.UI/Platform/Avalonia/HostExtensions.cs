using Avalonia;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Vectron.UI.Platform.Avalonia;

public static class HostExtensions
{
    [SupportedOSPlatform("macos")]
    public static int RunAvaloniaApp(this IHost host)
    {
        IHostedLifetime lifetime = host.Services.GetRequiredService<IHostedLifetime>();
        Application application = host.Services.GetRequiredService<Application>();
        int result = host.StartAsync(CancellationToken.None)
            .ContinueWith(_ => lifetime.StartAsync(application, CancellationToken.None).GetAwaiter().GetResult())
            .GetAwaiter().GetResult();

        Task.WaitAll(host.StopAsync(CancellationToken.None), host.WaitForShutdownAsync(CancellationToken.None));

        return result;
    }

    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public static async Task<int> RunAvaloniaAppAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        IHostedLifetime lifetime = host.Services.GetRequiredService<IHostedLifetime>();
        Application application = host.Services.GetRequiredService<Application>();
        await host.StartAsync(cancellationToken);
        int result = await lifetime.StartAsync(application, cancellationToken);
        await host.StopAsync(cancellationToken);
        await host.WaitForShutdownAsync(cancellationToken);
        return result;
    }
}