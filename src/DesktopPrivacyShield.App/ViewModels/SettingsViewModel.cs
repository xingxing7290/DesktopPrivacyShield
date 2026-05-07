using CommunityToolkit.Mvvm.ComponentModel;
using DesktopPrivacyShield.App.Infrastructure;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.App.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly IConfigService _configService;
    private readonly IPasswordService _passwordService;
    private readonly IStartupService _startupService;
    private readonly IHotkeyService _hotkeyService;
    private readonly IIdleService _idleService;

    [ObservableProperty] private bool _startWithWindows;
    [ObservableProperty] private bool _startLocked;
    [ObservableProperty] private bool _idleLockEnabled;
    [ObservableProperty] private int _idleLockMinutes;
    [ObservableProperty] private bool _hotkeyEnabled;
    [ObservableProperty] private bool _coverAllMonitors;
    [ObservableProperty] private bool _showClock;
    [ObservableProperty] private bool _showMessage;
    [ObservableProperty] private string _message = string.Empty;
    [ObservableProperty] private string _currentPassword = string.Empty;
    [ObservableProperty] private string _newPassword = string.Empty;
    [ObservableProperty] private string _confirmNewPassword = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public RelayAsyncCommand SaveCommand { get; }

    public SettingsViewModel(
        IConfigService configService,
        IPasswordService passwordService,
        IStartupService startupService,
        IHotkeyService hotkeyService,
        IIdleService idleService)
    {
        _configService = configService;
        _passwordService = passwordService;
        _startupService = startupService;
        _hotkeyService = hotkeyService;
        _idleService = idleService;
        SaveCommand = new RelayAsyncCommand(SaveAsync);
        Load();
    }

    public void Load()
    {
        var config = _configService.Current;
        StartWithWindows = config.App.StartWithWindows;
        StartLocked = config.App.StartLocked;
        IdleLockEnabled = config.Lock.IdleLockEnabled;
        IdleLockMinutes = config.Lock.IdleLockMinutes;
        HotkeyEnabled = config.Lock.HotkeyEnabled;
        CoverAllMonitors = config.Lock.CoverAllMonitors;
        ShowClock = config.Lock.ShowClock;
        ShowMessage = config.Lock.ShowMessage;
        Message = config.Lock.Message;
        StatusMessage = string.Empty;
    }

    private async Task SaveAsync()
    {
        StatusMessage = string.Empty;
        var config = _configService.Current;
        config.App.StartWithWindows = StartWithWindows;
        config.App.StartLocked = StartLocked;
        config.Lock.IdleLockEnabled = IdleLockEnabled;
        config.Lock.IdleLockMinutes = Math.Max(1, IdleLockMinutes);
        config.Lock.HotkeyEnabled = HotkeyEnabled;
        config.Lock.CoverAllMonitors = CoverAllMonitors;
        config.Lock.ShowClock = ShowClock;
        config.Lock.ShowMessage = ShowMessage;
        config.Lock.Message = Message.Trim();

        if (!string.IsNullOrWhiteSpace(NewPassword) || !string.IsNullOrWhiteSpace(ConfirmNewPassword))
        {
            if (NewPassword != ConfirmNewPassword)
            {
                StatusMessage = "新密码两次输入不一致。";
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                StatusMessage = "修改密码需要输入当前密码。";
                return;
            }

            var changed = await _passwordService.ChangePasswordAsync(CurrentPassword, NewPassword);
            if (!changed)
            {
                StatusMessage = "当前密码错误，密码未更新。";
                return;
            }

            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
        }

        await _configService.SaveAsync(config);
        _startupService.Apply(StartWithWindows);
        _hotkeyService.RefreshSettings();
        _idleService.RefreshSettings();
        StatusMessage = "设置已保存。";
    }
}
