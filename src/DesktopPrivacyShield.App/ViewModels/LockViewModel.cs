using CommunityToolkit.Mvvm.ComponentModel;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.App.ViewModels;

public sealed partial class LockViewModel : ObservableObject
{
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
}
