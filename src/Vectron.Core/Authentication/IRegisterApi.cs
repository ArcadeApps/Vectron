using System.Text.Json.Serialization;
using Refit;

namespace Vectron.Core.Authentication;

public interface IRegisterApi
{
    [Get("/_matrix/client/v3/register/available")]
    internal Task<ApiResponse<UsernameAvailabilityResponse>> CheckUsernameInternal([AliasAs("username")] string username);

    [Post("/_matrix/client/v3/register")]
    internal Task<IApiResponse> RegisterInternal(RegisterRequest? registerRequest = null);
    
    [Post("/_matrix/client/v3/register/email/requestToken")]
    internal Task<ApiResponse<ValidateTokenResponse>> ValidateEmailInternal([Body] ValidateEmailRequest request);

    [Post("/_matrix/client/v3/register/msisdn/requestToken")]
    internal Task<ApiResponse<ValidateTokenResponse>> ValidatePhoneInternal([Body] ValidatePhoneRequest request);

    Task Register(string username, string password);
    
    Task<Result<UsernameAvailabilityResponse>> CheckUsernameAsync(string username) =>
        CheckUsernameInternal(username).ToResult();

    Task<Result<ValidateTokenResponse>> ValidateEmailAsync(string secret, string email, int attempts = 1) =>
        ValidateEmailInternal(new(secret, email, attempts)).ToResult();

    Task<Result<ValidateTokenResponse>> ValidatePhoneAsync(string secret, string country, string phoneNumber, int attempts = 1) =>
        ValidatePhoneInternal(new(secret, country, phoneNumber, attempts)).ToResult();
}

internal record RegisterRequest(
    [property: JsonPropertyName("device_id")]
    string DeviceId,
    [property: JsonPropertyName("initial_device_display_name")]
    string InitialDeviceDisplayName,
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("password")]
    string Password,
    [property: JsonPropertyName("auth")]
    AuthData? Auth);

internal record AuthData(
    [property: JsonPropertyName("session")]
    string SessionId, 
    string Type,
    [property: JsonPropertyName("threepid_creds")]
    ThreePidCredentials? ThreePidCredentials);

internal record ThreePidCredentials(
    [property: JsonPropertyName("sid")]
    string Sid,
    [property: JsonPropertyName("client_secret")]
    string Secret,
    [property: JsonPropertyName("id_server")]
    string? IdServer,
    [property: JsonPropertyName("id_server_access_token")]
    string? IdServerToken);

internal record ValidatePhoneRequest(
    [property: JsonPropertyName("client_secret")]
    string Secret,
    [property: JsonPropertyName("country")]
    string Country,
    [property: JsonPropertyName("phone_number")]
    string Phone,
    [property: JsonPropertyName("send_attempt")]
    int Attempts);

internal record ValidateEmailRequest(
    [property: JsonPropertyName("client_secret")]
    string Secret,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("send_attempt")]
    int Attempts);

public record ValidateTokenResponse(string Sid);

public record UsernameAvailabilityResponse(bool Available);