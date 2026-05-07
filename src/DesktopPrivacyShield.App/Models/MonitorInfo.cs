namespace DesktopPrivacyShield.App.Models;

public sealed class MonitorInfo
{
    public required string DeviceName { get; init; }
    public required double Left { get; init; }
    public required double Top { get; init; }
    public required double Width { get; init; }
    public required double Height { get; init; }
    public required bool IsPrimary { get; init; }
}
