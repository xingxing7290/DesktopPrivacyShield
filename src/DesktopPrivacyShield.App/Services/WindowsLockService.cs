using DesktopPrivacyShield.App.Utils;

namespace DesktopPrivacyShield.App.Services;

public sealed class WindowsLockService : IWindowsLockService
{
    public void LockWorkStation() => Win32Helper.LockWorkStation();
}
