using System.Windows;
using DesktopPrivacyShield.App.Services;
using DesktopPrivacyShield.App.Utils;
using DesktopPrivacyShield.App.ViewModels;
using DesktopPrivacyShield.App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DesktopPrivacyShield.App;

public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .UseSerilog((context, services, config) => config
                .MinimumLevel.Information()
                .WriteTo.File(
                    PathHelper.GetLogPath(),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7))
            .ConfigureServices(services =>
            {
                services.AddSingleton<IConfigService, ConfigService>();
                services.AddSingleton<IPasswordService, PasswordService>();
                services.AddSingleton<IMonitorService, MonitorService>();
                services.AddSingleton<IStartupService, StartupService>();
                services.AddSingleton<IWindowsLockService, WindowsLockService>();
                services.AddSingleton<ILockService, LockService>();
                services.AddSingleton<IIdleService, IdleService>();
                services.AddSingleton<IHotkeyService, HotkeyService>();
                services.AddSingleton<ITrayService, TrayService>();
                services.AddSingleton<IApplicationCoordinator, ApplicationCoordinator>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<FirstRunViewModel>();
                services.AddSingleton<LockViewModel>();
                services.AddTransient<SettingsViewModel>();

                services.AddSingleton<MainWindow>();
                services.AddTransient<FirstRunWindow>();
                services.AddTransient<SettingsWindow>();
            })
            .Build();

        await _host.StartAsync();
        var coordinator = _host.Services.GetRequiredService<IApplicationCoordinator>();
        await coordinator.InitializeAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            foreach (var disposable in new IDisposable[]
                     {
                         _host.Services.GetRequiredService<ITrayService>(),
                         _host.Services.GetRequiredService<IHotkeyService>(),
                         _host.Services.GetRequiredService<IIdleService>()
                     })
            {
                disposable.Dispose();
            }

            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
