using CommunityToolkit.Mvvm.ComponentModel;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.App.ViewModels;

public sealed partial class LockViewModel : ObservableObject
{
    private readonly IPasswordService _passwordService;
    private readonly IConfigService _configService;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _showClock;

    [ObservableProperty]
    private bool _showMessage;

    [ObservableProperty]
    private string _overlayMessage = string.Empty;

    public LockViewModel(IPasswordService passwordService, IConfigService configService)
    {
        _passwordService = passwordService;
        _configService = configService;
        LoadSettings();
    }

    public void LoadSettings()
    {
        var options = _configService.Current.Lock;
        ShowClock = options.ShowClock;
        ShowMessage = options.ShowMessage;
        OverlayMessage = options.Message;
    }

    public async Task<bool> TryUnlockAsync(string password)
    {
        var ok = await _passwordService.VerifyPasswordAsync(password);
        if (!ok)
        {
            var delay = _passwordService.GetCurrentDelay();
            StatusMessage = delay > TimeSpan.Zero
                ? $"密码错误，请稍后重试。当前延迟 {delay.TotalSeconds:0} 秒。"
                : "密码错误。";
            return false;
        }

        Password = string.Empty;
        StatusMessage = string.Empty;
        return true;
    }
}
