using HeyGPT.App.ViewModels;
using HeyGPT.Core.Models;

namespace HeyGPT.App.Contracts.Services;

public interface ILoginService
{
    bool IsLoggedIn { get; }

    Task Login(OpenAISecretKey key);

    Task Logout();
}
