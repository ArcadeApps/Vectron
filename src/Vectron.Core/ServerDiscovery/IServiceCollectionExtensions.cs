using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Vectron.Core.Authentication;

namespace Vectron.Core.ServerDiscovery;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServerDiscovery(this IServiceCollection services, string matrixServerUrl)
    {
        if (!Uri.TryCreate(matrixServerUrl, UriKind.RelativeOrAbsolute, out var uri)) return services;
        services.AddRefitClient<IServerDiscoveryApi>().ConfigureHttpClient(c => c.BaseAddress = uri);
        services.AddRefitClient<IRegisterApi>().ConfigureHttpClient(c => c.BaseAddress = uri);

        return services;
    }
}