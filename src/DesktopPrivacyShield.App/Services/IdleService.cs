using DesktopPrivacyShield.App.Utils;
using System.Windows.Threading;

namespace DesktopPrivacyShield.App.Services;

public sealed class IdleService : IIdleService
{
    private readonly IConfigService _configService;
    private readonly ILockService _lockService;
    private readonly DispatcherTimer _timer;

    public IdleService(IConfigService configService, ILockService lockService)
    {
        _configService = configService;
        _lockService = lockService;
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        _timer.Tick += OnTick;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void RefreshSettings()
    {
        // Timer interval is fixed; settings are evaluated on each tick.
    }

    private async void OnTick(object? sender, EventArgs e)
    {
        var config = _configService.Current;
        if (!config.Lock.IdleLockEnabled || _lockService.IsLocked)
        {
            return;
        }

        var idle = Win32Helper.GetIdleTime();
        if (idle >= TimeSpan.FromMinutes(config.Lock.IdleLockMinutes))
        {
            await _lockService.LockAsync();
        }
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Tick -= OnTick;
    }
}
