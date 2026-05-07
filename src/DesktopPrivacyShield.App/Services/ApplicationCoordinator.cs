using System.Windows;
using DesktopPrivacyShield.App.ViewModels;
using DesktopPrivacyShield.App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DesktopPrivacyShield.App.Services;

public sealed class ApplicationCoordinator : IApplicationCoordinator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfigService _configService;
    private readonly IPasswordService _passwordService;
    private readonly ITrayService _trayService;
    private readonly IHotkeyService _hotkeyService;
    private readonly IIdleService _idleService;
    private readonly ILockService _lockService;
    private readonly IStartupService _startupService;
    private readonly ILogger<ApplicationCoordinator> _logger;
    private MainWindow? _mainWindow;
    private SettingsWindow? _settingsWindow;

    public ApplicationCoordinator(
        IServiceProvider serviceProvider,
        IConfigService configService,
        IPasswordService passwordService,
        ITrayService trayService,
        IHotkeyService hotkeyService,
        IIdleService idleService,
        ILockService lockService,
        IStartupService startupService,
        ILogger<ApplicationCoordinator> logger)
    {
        _serviceProvider = serviceProvider;
        _configService = configService;
        _passwordService = passwordService;
        _trayService = trayService;
        _hotkeyService = hotkeyService;
        _idleService = idleService;
        _lockService = lockService;
        _startupService = startupService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _configService.Load();
        _startupService.Apply(_configService.Current.App.StartWithWindows);
        _lockService.LockStateChanged += (_, isLocked) => _trayService.UpdateState(isLocked);

        _trayService.Initialize();
        _hotkeyService.Start();
        _idleService.Start();

        if (!_passwordService.HasPassword())
        {
            var firstRun = _serviceProvider.GetRequiredService<FirstRunWindow>();
            firstRun.ShowDialog();
        }

        _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _mainWindow.Show();

        if (_configService.Current.App.StartLocked)
        {
            await _lockService.LockAsync();
        }

        _logger.LogInformation("Application initialized.");
    }

    public void ShowSettings()
    {
        if (_settingsWindow is { IsVisible: true })
        {
            _settingsWindow.Activate();
            return;
        }

        _settingsWindow = _serviceProvider.GetRequiredService<SettingsWindow>();
        _settingsWindow.Closed += (_, _) => _settingsWindow = null;
        (_settingsWindow.DataContext as SettingsViewModel)?.Load();
        _settingsWindow.Show();
        _settingsWindow.Activate();
    }

    public void ShowAbout()
    {
        System.Windows.MessageBox.Show(
            "DesktopPrivacyShield\n\n桌面隐私遮罩工具。\n该软件不是系统级安全锁屏。",
            "关于",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public async Task ShutdownAsync()
    {
        if (_lockService.IsLocked)
        {
            return;
        }

        _logger.LogInformation("Application shutting down.");
        await Task.CompletedTask;
        System.Windows.Application.Current.Shutdown();
    }
}
