using System;
using Microsoft.Extensions.DependencyInjection;

namespace Vectron.UI.Utilities;

public static class DesignTimeHelper
{
    private static readonly Lazy<IServiceScope> Scope = new Lazy<IServiceScope>(() =>
    {
        _serviceProvider = Program.BuildServices();
        return _serviceProvider.CreateScope();
    });
        
    private static IServiceProvider? _serviceProvider;
    public static IServiceProvider Services => Scope.Value.ServiceProvider;
}