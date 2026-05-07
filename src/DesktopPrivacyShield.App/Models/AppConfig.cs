namespace DesktopPrivacyShield.App.Models;

public sealed class AppConfig
{
    public AppOptions App { get; set; } = new();
    public LockOptions Lock { get; set; } = new();
    public PasswordConfig Password { get; set; } = new();
    public SecurityOptions Security { get; set; } = new();
    public LoggingOptions Logging { get; set; } = new();
}

public sealed class AppOptions
{
    public string Language { get; set; } = "zh-CN";
    public bool StartWithWindows { get; set; }
    public bool StartLocked { get; set; }
}

public sealed class LockOptions
{
    public string Mode { get; set; } = "overlay";
    public string Theme { get; set; } = "dark";
    public bool IdleLockEnabled { get; set; }
    public int IdleLockMinutes { get; set; } = 10;
    public bool HotkeyEnabled { get; set; } = true;
    public string Hotkey { get; set; } = "Ctrl+Alt+L";
    public bool CoverAllMonitors { get; set; } = true;
    public bool ShowClock { get; set; } = true;
    public bool ShowMessage { get; set; } = true;
    public string Message { get; set; } = "AI 任务运行中，请勿操作";
}

public sealed class PasswordConfig
{
    public string Algorithm { get; set; } = "PBKDF2";
    public int Iterations { get; set; } = 100_000;
    public string Salt { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
}

public sealed class SecurityOptions
{
    public int MaxFailedAttemptsBeforeDelay { get; set; } = 3;
    public int DelaySeconds { get; set; } = 5;
    public bool AllowExitFromTrayWhenUnlocked { get; set; } = true;
}

public sealed class LoggingOptions
{
    public bool Enabled { get; set; } = true;
    public string Level { get; set; } = "Information";
    public int RetentionDays { get; set; } = 7;
}
