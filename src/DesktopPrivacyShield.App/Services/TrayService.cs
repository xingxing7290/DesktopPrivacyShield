using System.Drawing;
using Microsoft.Extensions.Logging;
using Forms = System.Windows.Forms;

namespace DesktopPrivacyShield.App.Services;

public sealed class TrayService : ITrayService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILockService _lockService;
    private readonly IWindowsLockService _windowsLockService;
    private readonly ILogger<TrayService> _logger;

    private readonly Forms.NotifyIcon _notifyIcon;
    private readonly Forms.ToolStripMenuItem _lockItem;
    private readonly Forms.ToolStripMenuItem _settingsItem;
    private readonly Forms.ToolStripMenuItem _windowsLockItem;
    private readonly Forms.ToolStripMenuItem _aboutItem;
    private readonly Forms.ToolStripMenuItem _exitItem;

    public TrayService(
        IServiceProvider serviceProvider,
        ILockService lockService,
        IWindowsLockService windowsLockService,
        ILogger<TrayService> logger)
    {
        _serviceProvider = serviceProvider;
        _lockService = lockService;
        _windowsLockService = windowsLockService;
        _logger = logger;

        _lockItem = new Forms.ToolStripMenuItem("立即遮罩", null, async (_, _) => await _lockService.LockAsync());
        _settingsItem = new Forms.ToolStripMenuItem("设置", null, (_, _) => ResolveCoordinator().ShowSettings());
        _windowsLockItem = new Forms.ToolStripMenuItem("使用 Windows 系统锁屏", null, (_, _) => _windowsLockService.LockWorkStation());
        _aboutItem = new Forms.ToolStripMenuItem("关于", null, (_, _) => ResolveCoordinator().ShowAbout());
        _exitItem = new Forms.ToolStripMenuItem("退出", null, async (_, _) => await ResolveCoordinator().ShutdownAsync());

        _notifyIcon = new Forms.NotifyIcon
        {
            Text = "Desktop Privacy Shield",
            Icon = SystemIcons.Shield,
            Visible = false,
            ContextMenuStrip = new Forms.ContextMenuStrip()
        };
        _notifyIcon.ContextMenuStrip.Items.AddRange(new Forms.ToolStripItem[]
        {
            _lockItem,
            _settingsItem,
            _windowsLockItem,
            _aboutItem,
            _exitItem
        });
        _notifyIcon.DoubleClick += (_, _) => ResolveCoordinator().ShowSettings();
    }

    private IApplicationCoordinator ResolveCoordinator() =>
        (IApplicationCoordinator)_serviceProvider.GetService(typeof(IApplicationCoordinator))!;

    public void Initialize()
    {
        _notifyIcon.Visible = true;
        UpdateState(_lockService.IsLocked);
        _logger.LogInformation("Tray service initialized.");
    }

    public void UpdateState(bool isLocked)
    {
        _lockItem.Enabled = !isLocked;
        _settingsItem.Enabled = !isLocked;
        _windowsLockItem.Enabled = !isLocked;
        _exitItem.Enabled = !isLocked;
        _notifyIcon.Text = isLocked ? "Desktop Privacy Shield - Locked" : "Desktop Privacy Shield";
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}
