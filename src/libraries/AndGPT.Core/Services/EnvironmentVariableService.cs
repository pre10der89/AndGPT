using AndGPT.Core.Contracts.Services;

namespace AndGPT.Core.Services;

public class EnvironmentVariableService : IEnvironmentVariableService
{
    public string GetEnvironmentVariable(string variableName)
    {
        // Try to get the environment variable from the user scope.
        var value = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);

        // If not found in the user scope, try the system scope.
        if (string.IsNullOrEmpty(value))
        {
            value = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
        }

        // Return the value found, or an empty string if none found.
        return value ?? string.Empty;
    }
}
