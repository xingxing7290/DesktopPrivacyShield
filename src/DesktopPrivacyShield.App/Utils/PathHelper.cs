using System.IO;

namespace DesktopPrivacyShield.App.Utils;

public static class PathHelper
{
    public static string GetAppDataDirectory()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) ?? string.Empty;
        var path = Path.Combine(
            appData,
            "DesktopPrivacyShield");
        Directory.CreateDirectory(path);
        return path;
    }

    public static string GetConfigPath() => Path.Combine(GetAppDataDirectory(), "config.json");

    public static string GetLogDirectory()
    {
        var path = Path.Combine(GetAppDataDirectory(), "logs");
        Directory.CreateDirectory(path);
        return path;
    }

    public static string GetLogPath() => Path.Combine(GetLogDirectory(), "app.log");
}
