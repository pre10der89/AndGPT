using AndGPT.Core.Contracts.Services;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Models;

namespace HeyGPT.Core.Services;

public class OpenAIService : IOpenAIService
{
    private const string ApiKeyEnvVariableName = @"HeyGPTKey";
    private const string ApiOrganizationEnvVariableName = @"HeyGPTOrganization";

    private readonly IEnvironmentVariableService _environmentVariableService;
    private OpenAISecretKey _secretKey = OpenAISecretKey.Empty;
    private string _organizationId = string.Empty;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OpenAIService(IEnvironmentVariableService environmentVariableService)
    {
        _environmentVariableService = environmentVariableService;
    }

    public async Task InitializeAsync()
    {
        // TODO: Use User Secrets
        var secretKey = _environmentVariableService.GetEnvironmentVariable(ApiKeyEnvVariableName);

        _secretKey = new OpenAISecretKey(secretKey);
        _organizationId = _environmentVariableService.GetEnvironmentVariable(ApiOrganizationEnvVariableName);

        await Task.CompletedTask;
    }
}
