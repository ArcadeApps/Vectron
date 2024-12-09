using Microsoft.Extensions.DependencyInjection;
using Vectron.Core.ServerDiscovery;

namespace Vectron.Core.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServerDiscovery("https://matrix.org/");
    }
}