using Refit;

namespace Vectron.Core.ServerDiscovery;
[Headers("Accept: application/json")]
public interface IServerDiscoveryApi
{
    [Get("/.well-known/matrix/client")]
    Task<ApiResponse<ServerClientInfoResponse>> GetServerClientInfoAsync();
    
    [Get("/.well-known/matrix/support")]
    Task GetServerSupportInfoAsync();
    
    [Get("/_matrix/client/versions")]
    Task<ApiResponse<VersionsResponse>> GetVersionsAsync();
}