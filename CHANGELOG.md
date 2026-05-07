# Changelog

All notable changes to this project will be documented in this file.

## [0.2.0] - 2026-05-07

### Added

- Automatic Windows system lock option that invokes `LockWorkStation` after the overlay is shown
- New generated brand logo integrated into the application icon, window icon, and tray icon
- Unified visual theme resources for the WPF application

### Changed

- Modernized main window, first-run setup window, settings window, and lock overlay UI
- Improved settings structure and explanatory text for system lock behavior
- Refreshed installer output and release artifacts for the new version

## [0.1.0] - 2026-05-07

### Added

- Initial `.NET 8 + WPF` desktop application scaffold
- Fullscreen privacy overlay for Windows desktop sessions
- Multi-monitor coverage with primary-screen password entry
- PBKDF2 password hashing and local JSON configuration
- Failed unlock attempt throttling
- System tray menu and global hotkey `Ctrl + Alt + L`
- Idle-triggered overlay locking
- Windows startup registration support
- Windows native lock entry point via `LockWorkStation`
- First-run password setup flow
- Settings window for core runtime options
- Serilog local file logging
- xUnit tests for password service behavior
- Inno Setup installer script
