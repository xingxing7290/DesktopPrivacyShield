using DesktopPrivacyShield.App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DesktopPrivacyShield.App.Services;

public sealed class LockService : ILockService
{
    private readonly IConfigService _configService;
    private readonly IPasswordService _passwordService;
    private readonly IMonitorService _monitorService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LockService> _logger;
    private readonly List<LockWindow> _windows = [];
    private bool _subscribed;

    public LockService(
        IConfigService configService,
        IPasswordService passwordService,
        IMonitorService monitorService,
        IServiceProvider serviceProvider,
        ILogger<LockService> logger)
    {
        _configService = configService;
        _passwordService = passwordService;
        _monitorService = monitorService;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public bool IsLocked { get; private set; }

    public event EventHandler<bool>? LockStateChanged;

    public Task LockAsync()
    {
        if (IsLocked)
        {
            return Task.CompletedTask;
        }

        IsLocked = true;
        EnsureSystemEvents();
        CreateWindows();
        _logger.LogInformation("Lock mode entered.");
        LockStateChanged?.Invoke(this, true);
        return Task.CompletedTask;
    }

    public async Task UnlockAsync(string password)
    {
        var success = await _passwordService.VerifyPasswordAsync(password);
        if (!success)
        {
            foreach (var window in _windows.Where(w => w.IsPrimaryWindow))
            {
                window.SetStatusMessage(_passwordService.GetCurrentDelay());
            }

            _logger.LogWarning("Unlock failed. FailedAttempts={FailedAttempts}", _passwordService.FailedAttempts);
            return;
        }

        CloseAllWindows();
        IsLocked = false;
        _logger.LogInformation("Unlock succeeded.");
        LockStateChanged?.Invoke(this, false);
    }

    public Task ForceRefreshOverlayAsync()
    {
        if (!IsLocked)
        {
            return Task.CompletedTask;
        }

        CloseAllWindows();
        CreateWindows();
        return Task.CompletedTask;
    }

    private void EnsureSystemEvents()
    {
        if (_subscribed)
        {
            return;
        }

        SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
        _subscribed = true;
    }

    private async void OnDisplaySettingsChanged(object? sender, EventArgs e)
    {
        await ForceRefreshOverlayAsync();
    }

    private void CreateWindows()
    {
        var monitors = _configService.Current.Lock.CoverAllMonitors
            ? _monitorService.GetAllMonitors()
            : [_monitorService.GetPrimaryMonitor()];

        foreach (var monitor in monitors)
        {
            var window = ActivatorUtilities.CreateInstance<LockWindow>(_serviceProvider, monitor, monitor.IsPrimary);
            _windows.Add(window);
            window.Show();
            window.Activate();
        }
    }

    private void CloseAllWindows()
    {
        foreach (var window in _windows.ToList())
        {
            window.Close();
        }

        _windows.Clear();
    }
}
