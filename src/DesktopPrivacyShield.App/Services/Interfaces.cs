using DesktopPrivacyShield.App.Models;

namespace DesktopPrivacyShield.App.Services;

public interface IConfigService
{
    AppConfig Current { get; }
    bool ConfigExists();
    AppConfig Load();
    Task SaveAsync(AppConfig config);
    string GetConfigPath();
}

public interface IPasswordService
{
    bool HasPassword();
    Task SetPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password);
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    int FailedAttempts { get; }
    TimeSpan GetCurrentDelay();
    void ResetFailedAttempts();
}

public interface IMonitorService
{
    IReadOnlyList<MonitorInfo> GetAllMonitors();
    MonitorInfo GetPrimaryMonitor();
}

public interface ILockService
{
    bool IsLocked { get; }
    event EventHandler<bool>? LockStateChanged;
    Task LockAsync();
    Task UnlockAsync(string password);
    Task ForceRefreshOverlayAsync();
}

public interface ITrayService : IDisposable
{
    void Initialize();
    void UpdateState(bool isLocked);
}

public interface IIdleService : IDisposable
{
    void Start();
    void RefreshSettings();
}

public interface IHotkeyService : IDisposable
{
    void Start();
    void RefreshSettings();
}

public interface IStartupService
{
    void Apply(bool enabled);
    bool IsEnabled();
}

public interface IWindowsLockService
{
    void LockWorkStation();
}

public interface IApplicationCoordinator
{
    Task InitializeAsync();
    void ShowSettings();
    void ShowAbout();
    Task ShutdownAsync();
}
