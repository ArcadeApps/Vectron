using System.Text.Json.Serialization;

namespace Vectron.Core.ServerDiscovery;

public record ServerClientInfoResponse(
    [property: JsonPropertyName("m.homeserver")]HomeServerInfo HomeServer, 
    [property: JsonPropertyName("m.identity_server")]IdentityServerInfo IdentityServer);
public record HomeServerInfo(
    [property: JsonPropertyName("base_url")]string BaseUrl);
public record IdentityServerInfo(
    [property: JsonPropertyName("base_url")]string BaseUrl);