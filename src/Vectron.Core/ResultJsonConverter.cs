using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vectron.Core;

public class ResultJsonConverter : JsonConverter<Result>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Result).IsAssignableFrom(typeToConvert);
    }

    public override Result? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var innerType = typeToConvert.GetGenericArguments()[0];
        
        var value = JsonSerializer.Deserialize(ref reader, innerType, options);

        var returnType = typeof(Result<>).MakeGenericType(innerType);
        var createMethodType = typeof(Result).GetMethods().Single(x => x is { Name: "Success", IsGenericMethodDefinition: true });
        var createMethod = createMethodType.MakeGenericMethod(innerType);
        var output = createMethod?.Invoke(null, [value]);
        return (Result)output;
    }

    public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
    {
        if(value.IsFailure) throw new JsonException("Failure results cannot be converted to JSON");
        throw new NotImplementedException();
    }
}