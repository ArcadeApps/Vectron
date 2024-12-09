using System.Text.Json.Serialization;

namespace Vectron.Core.ServerDiscovery;

[JsonConverter(typeof(VersionsResponseConverter))]
public record VersionsResponse(string[] Versions, (string, bool)[] UnstableFeatures);