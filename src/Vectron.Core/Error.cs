using System.Text.Json.Serialization;

namespace Vectron.Core;

public sealed record Error(
    [property: JsonPropertyName("errcode")]string Code, 
    [property: JsonPropertyName("error")]string Message)
{
    public static Error None => new(string.Empty, string.Empty);
    
    public static implicit operator string(Error? error) => error?.Code ?? string.Empty;

    public bool Equals(Error? other)
    {
        if(other is null) return false;
        
        return Code == other.Code && Message == other.Message;
    }
    
    public override int GetHashCode() => HashCode.Combine(Code, Message);
}