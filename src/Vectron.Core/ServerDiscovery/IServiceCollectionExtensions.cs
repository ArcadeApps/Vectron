using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Vectron.Core.Authentication;

namespace Vectron.Core.ServerDiscovery;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServerDiscovery(this IServiceCollection services)
    {
        services.AddRefitClient<IServerDiscoveryApi>();
        services.AddRefitClient<IRegisterApi>();
        return services;
    }
}