using System.Windows.Forms;
using DesktopPrivacyShield.App.Utils;
using Microsoft.Extensions.Logging;

namespace DesktopPrivacyShield.App.Services;

public sealed class HotkeyService : IHotkeyService
{
    private const int HotkeyId = 1107;

    private readonly IConfigService _configService;
    private readonly ILockService _lockService;
    private readonly ILogger<HotkeyService> _logger;
    private readonly HotkeyWindow _window;
    private bool _registered;

    public HotkeyService(IConfigService configService, ILockService lockService, ILogger<HotkeyService> logger)
    {
        _configService = configService;
        _lockService = lockService;
        _logger = logger;
        _window = new HotkeyWindow(async () => await _lockService.LockAsync());
    }

    public void Start() => RefreshSettings();

    public void RefreshSettings()
    {
        Unregister();
        if (!_configService.Current.Lock.HotkeyEnabled)
        {
            return;
        }

        _registered = Win32Helper.RegisterHotKey(
            _window.Handle,
            HotkeyId,
            Win32Helper.ModControl | Win32Helper.ModAlt,
            Win32Helper.VkL);

        if (!_registered)
        {
            _logger.LogWarning("Failed to register global hotkey Ctrl+Alt+L.");
        }
    }

    private void Unregister()
    {
        if (_registered)
        {
            Win32Helper.UnregisterHotKey(_window.Handle, HotkeyId);
            _registered = false;
        }
    }

    public void Dispose()
    {
        Unregister();
        _window.DestroyHandle();
    }

    private sealed class HotkeyWindow : NativeWindow
    {
        private readonly Func<Task> _onHotkey;

        public HotkeyWindow(Func<Task> onHotkey)
        {
            _onHotkey = onHotkey;
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32Helper.WmHotKey)
            {
                _ = _onHotkey();
            }

            base.WndProc(ref m);
        }
    }
}
