using DesktopPrivacyShield.App.Models;
using Forms = System.Windows.Forms;

namespace DesktopPrivacyShield.App.Services;

public sealed class MonitorService : IMonitorService
{
    public IReadOnlyList<MonitorInfo> GetAllMonitors()
    {
        return Forms.Screen.AllScreens
            .Select(screen => new MonitorInfo
            {
                DeviceName = screen.DeviceName,
                Left = screen.Bounds.Left,
                Top = screen.Bounds.Top,
                Width = screen.Bounds.Width,
                Height = screen.Bounds.Height,
                IsPrimary = screen.Primary
            })
            .ToList();
    }

    public MonitorInfo GetPrimaryMonitor() => GetAllMonitors().First(m => m.IsPrimary);
}
