using Microsoft.Win32;

namespace DesktopPrivacyShield.App.Services;

public sealed class StartupService : IStartupService
{
    private const string RunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "DesktopPrivacyShield";

    public void Apply(bool enabled)
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunPath);
        if (key is null)
        {
            return;
        }

        if (enabled)
        {
            var exePath = Environment.ProcessPath;
            if (!string.IsNullOrWhiteSpace(exePath))
            {
                key.SetValue(AppName, $"\"{exePath}\"");
            }
        }
        else
        {
            key.DeleteValue(AppName, false);
        }
    }

    public bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunPath);
        return key?.GetValue(AppName) is string;
    }
}
