# DesktopPrivacyShield v0.2.0

Modernized UI and branding release for DesktopPrivacyShield, with optional Windows system lock after overlay activation.

## Highlights

- New generated brand logo integrated into app and tray icons
- Refreshed desktop UI across the main window, setup flow, settings, and lock overlay
- Optional automatic Windows system lock after overlay activation
- Continued support for multi-monitor overlay, password unlock, tray control, and hotkey lock

## Assets

- `DesktopPrivacyShield-Setup-0.2.0.exe`
- `SHA256SUMS.txt`

## Notes

- The Windows system lock is not rendered as an in-app page layer.
- The app first shows its overlay, then invokes `LockWorkStation()`.
- After the user unlocks Windows, the app overlay remains active until the app password is entered.
