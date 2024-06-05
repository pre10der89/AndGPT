using Windows.Storage;
using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Helpers;
using AndGPT.UI.Core.Contracts.Contracts;
using AndGPT.UI.Core.Helpers;
using AndGPT.UI.Core.Models;
using Microsoft.Extensions.Options;

namespace AndGPT.UI.Core.Services;

public sealed class LocalSettingsService : ILocalSettingsService
{
    #region Fields

    private const string DefaultApplicationDataFolder = "ApplicationData";
    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;
    private readonly IDataProtectionService _dataProtectionService;
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _localSettingsFile;

    private IDictionary<string, object> _settings;

    private bool _isInitialized;

    #endregion

    #region Constructor(s)

    public LocalSettingsService(IFileService fileService, IDataProtectionService dataProtectionService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _dataProtectionService = dataProtectionService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
        _localSettingsFile = _options.LocalSettingsFile ?? DefaultLocalSettingsFile;

        _settings = new Dictionary<string, object>();
    }

    #endregion

    #region ILocalSettingsService Members

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        var valueString = await GetSettingInternalAsync(key);

        return await ConvertToObjectAsync<T>(valueString);
    }

    public async Task<T?> ReadSecretAsync<T>(string key)
    {
        var valueString = await GetSettingInternalAsync(key);

        return await DecryptObjectFromBase64<T>(valueString);
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        var stringValue = await ConvertToStringAsync(value);

        await SaveSettingInternalAsync(key, stringValue);

    }

    public async Task SaveSecretAsync<T>(string key, T value)
    {
        var encryptedString = await EncryptObjectToBase64(value);

        await SaveSettingInternalAsync(key, encryptedString);
    }

    public async Task RemoveSettingAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        if (RuntimeHelper.IsMSIX)
        {
            await InitializeWindowStorageAsync();

            ApplicationData.Current.LocalSettings.Values.Remove(key);
        }
        else
        {
            await InitializeStandAloneAsync();

            _settings.Remove(key);

            await Task.Run(() => _fileService.Save(_applicationDataFolder, _localSettingsFile, _settings));
        }
    }

    #endregion

    #region Private Methods

    private async Task InitializeWindowStorageAsync()
    {
        if (!_isInitialized)
        {
            // https://github.com/brunolemos/flatnotes/blob/9d67f19f8914e1b19890b43fc6072bb89007e75e/FlatNotes.Shared/Utils/Migration/Migration.cs
            //await ApplicationData.Current.SetVersionAsync(1, request =>
            //{
            //    if (request.DesiredVersion < 2)
            //    {
            //        if (request.CurrentVersion == 1)
            //        {

            //        }
            //    }

            //    //request.
            //});

            await Task.CompletedTask;

            _isInitialized = true;
        }
    }

    private async Task InitializeStandAloneAsync()
    {
        if (!_isInitialized)
        {
            _settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(_applicationDataFolder, _localSettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }

    private async Task<string?> GetSettingInternalAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        string? valueString = null;

        if (RuntimeHelper.IsMSIX)
        {
            await InitializeWindowStorageAsync();

            // ApplicationDataContainer
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
            {
                valueString = (string)obj;
            }
        }
        else
        {
            await InitializeStandAloneAsync();

            if (_settings.TryGetValue(key, out var obj))
            {
                valueString = (string)obj;
            }
        }

        return valueString;
    }

    private async Task SaveSettingInternalAsync(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        if (RuntimeHelper.IsMSIX)
        {
            await InitializeWindowStorageAsync();

            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
        else
        {
            await InitializeStandAloneAsync();

            _settings[key] = value;

            await Task.Run(() => _fileService.Save(_applicationDataFolder, _localSettingsFile, _settings));
        }
    }

    private async Task<string> ConvertToStringAsync<T>(T value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return await Json.StringifyAsync(value);
    }

    private async Task<T?> ConvertToObjectAsync<T>(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        return await Json.ToObjectAsync<T>(value);
    }

    private async Task<T?> DecryptObjectFromBase64<T>(string? base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return default;
        }

        var jsonString = await _dataProtectionService.DecryptStringFromBase64Async(base64);

        return !string.IsNullOrWhiteSpace(jsonString) ? await Json.ToObjectAsync<T>(jsonString) : default;
    }

    private async Task<string> EncryptObjectToBase64<T>(T value)
    {
        var stringValue = await ConvertToStringAsync(value);

        return await _dataProtectionService.EncryptStringToBase64Async(stringValue);
    }

    #endregion
}
