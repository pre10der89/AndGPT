namespace AndGPT.UI.Core.Contracts.Contracts;

public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task<T?> ReadSecretAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    Task SaveSecretAsync<T>(string key, T value);

    Task RemoveSettingAsync(string key);
}
