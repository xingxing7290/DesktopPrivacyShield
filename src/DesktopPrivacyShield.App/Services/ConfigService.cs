using System.IO;
using System.Text.Json;
using DesktopPrivacyShield.App.Models;
using DesktopPrivacyShield.App.Utils;

namespace DesktopPrivacyShield.App.Services;

public sealed class ConfigService : IConfigService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AppConfig Current { get; private set; } = new();

    public bool ConfigExists() => File.Exists(GetConfigPath());

    public AppConfig Load()
    {
        var path = GetConfigPath();
        if (!File.Exists(path))
        {
            Current = new AppConfig();
            return Current;
        }

        var content = File.ReadAllText(path);
        Current = JsonSerializer.Deserialize<AppConfig>(content, JsonOptions) ?? new AppConfig();
        return Current;
    }

    public async Task SaveAsync(AppConfig config)
    {
        Current = config;
        var json = JsonSerializer.Serialize(config, JsonOptions);
        await File.WriteAllTextAsync(GetConfigPath(), json);
    }

    public string GetConfigPath() => PathHelper.GetConfigPath();
}
