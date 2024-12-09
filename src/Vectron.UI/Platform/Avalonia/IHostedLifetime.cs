using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Logging;

namespace Vectron.UI.Platform.Avalonia;

public interface IHostedLifetime
{
    Task<int> StartAsync(Application application, CancellationToken cancellationToken);

    Task StopAsync(Application application, CancellationToken cancellationToken);
}

internal static class HostedLifetime
{
    internal static IHostedLifetime Select(ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime) =>
        applicationLifetime switch
        {
            IClassicDesktopStyleApplicationLifetime desktop =>
                new DesktopHostedLifetime(loggerFactory.CreateLogger<DesktopHostedLifetime>(), desktop),
            IControlledApplicationLifetime controlled =>
                new ControlledHostedLifetime(loggerFactory.CreateLogger<ControlledHostedLifetime>(), controlled),
            ISingleViewApplicationLifetime => throw new PlatformNotSupportedException(
                "This is only supposed to run on windows, linux and macos. Mobile lifetimes are now supported."),
            _ => new FallbackLifetime(loggerFactory.CreateLogger<FallbackLifetime>())
        };
}

internal abstract class HostedLifetimeBase<TRuntime> : IHostedLifetime where TRuntime : IApplicationLifetime
{
    public int ExitCode { get; } = 0;

    protected TRuntime Runtime { get; private init; }

    protected HostedLifetimeBase(TRuntime runtime)
    {
        Runtime = runtime;
    }

    public abstract Task<int> StartAsync(Application application, CancellationToken cancellationToken);
    public abstract Task StopAsync(Application application, CancellationToken cancellationToken);
}

internal sealed class FallbackLifetime : IHostedLifetime
{
    private readonly ILogger<FallbackLifetime> _logger;

    public FallbackLifetime(ILogger<FallbackLifetime> logger)
    {
        _logger = logger;
    }

    public Task<int> StartAsync(Application application, CancellationToken cancellationToken)
    {
        int RunWithCancellatioToken()
        {
            try
            {
                application.Run(cancellationToken);
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure while running the application.");
            }

            return -1;
        }

        return Task.Run(RunWithCancellatioToken, cancellationToken);
    }

    public Task StopAsync(Application application, CancellationToken cancellationToken)
    {
        return Task.FromException<NotSupportedException>(new NotSupportedException());
    }
}

internal class ControlledHostedLifetime : HostedLifetimeBase<IControlledApplicationLifetime>
{
    private readonly ILogger<IHostedLifetime> _logger;

    public ControlledHostedLifetime(ILogger<ControlledHostedLifetime> logger,
        IControlledApplicationLifetime controlled) : base(controlled)
    {
        _logger = logger;
    }

    public ControlledHostedLifetime(ILogger<IHostedLifetime> logger,
        IControlledApplicationLifetime controlled) : base(controlled)
    {
        _logger = logger;
    }

    public override Task<int> StartAsync(Application application, CancellationToken cancellationToken)
    {
        int RunInControlledBackground()
        {
            try
            {
                application.Run(cancellationToken);
                return 0;
            }
            catch (Exception e)
            {
                if (_logger.IsEnabled(LogLevel.Critical))
                    _logger.LogCritical(e, "Failure while running the application.");
                return -1;
            }
        }

        return Task.Run(RunInControlledBackground, cancellationToken);
    }

    public override Task StopAsync(Application application, CancellationToken cancellationToken)
    {
        return Task.Run(() => Runtime.Shutdown(), cancellationToken);
    }
}

internal class DesktopHostedLifetime : HostedLifetimeBase<IClassicDesktopStyleApplicationLifetime>
{
    private readonly ILogger<DesktopHostedLifetime> _logger;

    public DesktopHostedLifetime(ILogger<DesktopHostedLifetime> logger, IClassicDesktopStyleApplicationLifetime desktop)
        : base(desktop)
    {
        _logger = logger;
    }

    public override async Task<int> StartAsync(Application application, CancellationToken cancellationToken)
    {
        if (Runtime is ClassicDesktopStyleApplicationLifetime desktop)
        {
            int result;
            try
            {
                result = desktop.Start(desktop.Args ?? []);
            }
            catch (Exception e)
            {
                if (_logger.IsEnabled(LogLevel.Critical))
                    _logger.LogCritical(e, "Failure while running the application.");
                result = -1;
            }

            return await Task.FromResult(result);
        }

        return await new ControlledHostedLifetime(_logger, Runtime).StartAsync(application, cancellationToken);
    }

    public override async Task StopAsync(Application application, CancellationToken cancellationToken)
    {
        Window? mainWindow = Runtime.MainWindow;
        if (mainWindow is not null)
        {
            switch (Runtime.ShutdownMode)
            {
                case ShutdownMode.OnLastWindowClose:
                    foreach (var window in Runtime.Windows)
                    {
                        if (!ReferenceEquals(mainWindow, window))
                            await Task.Run(window.Close, cancellationToken);
                    }

                    await Task.Run(mainWindow.Close, cancellationToken);
                    return;
                case ShutdownMode.OnMainWindowClose:
                    await Task.Run(mainWindow.Close, cancellationToken);
                    return;
                case ShutdownMode.OnExplicitShutdown:
                    await Task.Run(() => Runtime.Shutdown(), cancellationToken);
                    return;
            }
        }

        await new ControlledHostedLifetime(_logger, Runtime).StopAsync(application, cancellationToken);
    }
}