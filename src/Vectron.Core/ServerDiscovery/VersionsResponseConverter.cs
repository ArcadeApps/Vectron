using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vectron.Core.ServerDiscovery;

public class VersionsResponseConverter : JsonConverter<VersionsResponse>
{
    public override VersionsResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<string> versions = [];
        List<(string, bool)> unstableFeatures = [];
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            var propertyName = reader.GetString();
            switch (propertyName)
            {
                case "versions":
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.StartArray) continue;
                        if (reader.TokenType == JsonTokenType.EndArray)
                            break;
                        var value = reader.GetString();
                        if (!string.IsNullOrEmpty(value))
                            versions.Add(value);
                    }

                    break;
                case "unstable_features":
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.StartObject) continue;
                        if (reader.TokenType == JsonTokenType.EndObject)
                            break;
                        var name = reader.GetString();
                        if (string.IsNullOrEmpty(name)) continue;
                        reader.Read();
                        unstableFeatures.Add((name, reader.TokenType == JsonTokenType.True));
                    }

                    break;
            }
        }

        return new VersionsResponse(versions.ToArray(), unstableFeatures.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, VersionsResponse value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}