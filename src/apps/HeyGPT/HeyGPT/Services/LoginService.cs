using HeyGPT.App.Contracts.Services;
using HeyGPT.Core.Models;

namespace HeyGPT.App.Services;

public class LoginService : ILoginService
{
    public bool IsLoggedIn
    {
        get;
    }

    public Task Login(OpenAISecretKey key) => throw new NotImplementedException();

    public Task Logout() => throw new NotImplementedException();
}
