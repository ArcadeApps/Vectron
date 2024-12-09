using Vectron.Core.Authentication;

namespace Vectron.Core.Tests;

public class UnitTest1
{
    private readonly IRegisterApi _registerApi;

    public UnitTest1(IRegisterApi registerApi)
    {
        _registerApi = registerApi;
    }
    
    [Fact]
    public async Task RegisterAPI_ValidateEmail()
    {
        var result = await _registerApi.ValidateEmailAsync("secret", "test@test.com");
        Assert.True(result.IsSuccess);
    }
}