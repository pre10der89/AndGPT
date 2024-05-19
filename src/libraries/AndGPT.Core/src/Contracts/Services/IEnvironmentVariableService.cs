namespace AndGPT.Core.Contracts.Services;

public interface IEnvironmentVariableService
{
    /// <summary>
    /// Gets the value of an environment variable from the user or system scope.
    /// </summary>
    /// <param name="variableName">The name of the environment variable.</param>
    /// <returns>The value of the environment variable or an empty string if not found.</returns>
    string GetEnvironmentVariable(string variableName);
}
